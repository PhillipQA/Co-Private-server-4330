"""Makes a 4330 Conquer.exe draw the Super quality shine for Legendary display items.

Usage:
    python patch_conquer_exe.py <unpacked Conquer.exe> <output exe>

The shipped Conquer.exe is UPX-packed with its "UPX0" section and "UPX!" marker renamed. To unpack it, rename the
first section from "afe0" to "UPX0" and the "fea!" marker to "UPX!", then run `upx -d`.

The client decides an item's quality shine from the last digit of its item type ID (6-9). Legendary items are shown
with display IDs ending in 1 (see Conquer.Game/Models/LegendaryLook.cs), so every quality check is redirected
through a small helper (source: legendary_shine.s) that maps a display ID back to its Super ID.
"""
import re
import struct
import sys

HELPER = bytes.fromhex(
    "3dbc420600750883c009e9a70000003db66d0600750883c007e9980000003d4ea20700750883c007e9890000005031d2"
    "b90a000000f7f183fa01757931d2b964000000f7f183f86f726b3da000000076153d90010000725d3d5702000076073d"
    "84030000754f8b04243dbd42060074453d21430600743e3d8543060074373de943060074303d4d44060074293db14406"
    "0074223d15450600741b3db56d060074143d4da20700740d3d5193080074065883c008eb015831d2b90a000000c3")

TEXT_VA, TEXT_RAW, HELPER_VA, TEXT_SIZE = 0x401000, 0x1000, 0x4DF800, 0xDF000
QUALITY_CHECK = re.compile(rb"\x33\xd2\xb9\x0a\x00\x00\x00\xf7\xf1\x89\x55(.)\x8b\x55\1\x83\xea\x06", re.S)


def main(source, target):
    original = open(source, "rb").read()
    exe = bytearray(original)
    helper_at = HELPER_VA - TEXT_VA + TEXT_RAW
    if exe[helper_at:helper_at + len(HELPER)] != bytes(len(HELPER)):
        sys.exit("Unexpected Conquer.exe: the spare space for the helper is not empty (already patched?).")
    exe[helper_at:helper_at + len(HELPER)] = HELPER

    sites = 0
    for match in QUALITY_CHECK.finditer(original):
        at = match.start()
        va = at - TEXT_RAW + TEXT_VA
        exe[at:at + 7] = b"\xe8" + struct.pack("<i", HELPER_VA - (va + 5)) + b"\x90\x90"
        sites += 1

    pe = struct.unpack_from("<I", exe, 0x3C)[0]
    section = pe + 24 + struct.unpack_from("<H", exe, pe + 20)[0]
    if not exe[section:section + 8].startswith(b".text"):
        sys.exit("Unexpected Conquer.exe layout.")
    struct.pack_into("<I", exe, section + 8, TEXT_SIZE)

    open(target, "wb").write(exe)
    print(f"Patched {sites} quality checks.")


if __name__ == "__main__":
    if len(sys.argv) != 3:
        sys.exit(__doc__)
    main(sys.argv[1], sys.argv[2])
