"""Adds the Legendary system to a 4330 client's data files.

Usage:
    python make_legendary_client.py <client folder> <output folder>

Reads  <client>/ini/itemtype.dat, <client>/ini/weapon.ini, <client>/ani/ItemMinIcon.Ani, <client>/ani/MapItemIcon.Ani
Writes the same four files to <output>, ready to copy over the client's own.

It adds:
  * the Dragon Shard, Super Dragon Ball and Legend Orb items (with icons borrowed from Meteor, Dragon Ball and
    Super Violet Gem), and
  * a "display" twin of every Super equipment item named "L-<name>" whose description lists its Legendary powers.
    The server sends the twin's ID for Legendary items (see Conquer.Game/Models/LegendaryLook.cs).

Run it on the client's ORIGINAL files; running it twice on its own output is harmless (existing IDs are skipped).
If you change the Legendary numbers in Player.LegendaryEffects.cs / Player.Legendary.cs, update TEXT below and rerun.
"""
import re
import struct
import sys
from pathlib import Path

RECORD = 204
NAME = slice(4, 20)
DESCRIPTION = slice(76, 204)
PRICE = 36

EXCEPTIONS = {410309: 410300, 421309: 421302, 500309: 500302}  # same as LegendaryLook.Exceptions

STATS = "Legendary:~+20%~Atk,~Def,~Magic~and~HP."
TEXT = {
    "weapon": "12%~chance~to~strike~again~for~+50%~damage.",
    "ring": "Lifesteal~5%~vs~players,~0.5%~vs~monsters~(max~2000).~-25%~stamina.~3x~passive~reach.",
    "necklace": "15%~chance~on~hit~to~restore~5%~mana.",
    "guard": "4%~chance~per~piece~to~heal~6%~HP~when~struck.",
}

MATERIALS = [
    (1088100, "DragonShard", 5000, "Dropped~by~bosses.~7~shards~and~3~Super~Dragon~Balls~make~a~Legend~Orb.", 1088001),
    (1088101, "SuperDragonBall", 100000, "Made~from~10~Dragon~Balls.~30%~chance~to~forge~Super~gear~into~Legendary~gear.", 1088000),
    (1088102, "LegendOrb", 300000, "Forges~Super~gear~into~Legendary~gear~(100%~success).~See~the~Legendsmith~in~Twin~City.", 700053),
]


def is_equipment(item_id):
    sub = item_id // 1000
    return 111 <= sub <= 160 or 400 <= sub <= 599 or sub == 900


def display_id(item_id):
    return EXCEPTIONS.get(item_id, item_id - 8)


def power_text(item_id):
    sub = item_id // 1000
    if 120 <= sub <= 121:
        return TEXT["necklace"]
    if 150 <= sub <= 152:
        return TEXT["ring"]
    if 400 <= sub <= 599:
        return TEXT["weapon"]
    return TEXT["guard"]  # helmets, earrings, caps, armor, boots, shields


def legendary_name(name):
    tagged = "L-" + name
    return tagged[:15]


def read_items(path):
    data = path.read_bytes()
    count = struct.unpack_from("<I", data, 0)[0]
    ids = list(struct.unpack_from(f"<{count}I", data, 4))
    start = 4 + 4 * count
    if start + count * RECORD != len(data):
        sys.exit(f"{path} is not the 204-byte binary itemtype.dat this script understands.")
    records = [bytearray(data[start + k * RECORD:start + (k + 1) * RECORD]) for k in range(count)]
    return ids, records


def write_items(path, ids, records):
    path.write_bytes(struct.pack("<I", len(ids)) + struct.pack(f"<{len(ids)}I", *ids) + b"".join(bytes(r) for r in records))


def make_record(template, item_id, name, description, price=None):
    record = bytearray(template)
    struct.pack_into("<I", record, 0, item_id)
    encoded_name, encoded_description = name.encode("latin1"), description.encode("latin1")
    assert len(encoded_name) <= 15 and len(encoded_description) <= 127, (name, len(encoded_description))
    record[NAME] = encoded_name.ljust(16, b"\0")
    record[DESCRIPTION] = encoded_description.ljust(128, b"\0")
    if price is not None:
        struct.pack_into("<I", record, PRICE, price)
    return record


def sections(text):
    """Maps section name -> body (without the header line) for an .ini/.ani file."""
    found = {}
    for match in re.finditer(r"^\[([^\]\r\n]+)\]\r?\n(.*?)(?=^\[|\Z)", text, re.S | re.M):
        found[match.group(1)] = match.group(2).rstrip("\r\n")
    return found


def append_sections(text, additions):
    if not additions:
        return text
    body = text.rstrip("\r\n")
    return body + "".join(f"\r\n\r\n[{name}]\r\n{content}" for name, content in additions) + "\r\n"


def main(client, out):
    client, out = Path(client), Path(out)
    (out / "ini").mkdir(parents=True, exist_ok=True)
    (out / "ani").mkdir(parents=True, exist_ok=True)

    ids, records = read_items(client / "ini" / "itemtype.dat")
    by_id = {item_id: records[k] for k, item_id in enumerate(ids)}
    icon_source = {}

    for item_id, name, price, description, icon in MATERIALS:
        if item_id not in by_id:
            record = make_record(by_id[1088000], item_id, name, description, price)
            struct.pack_into("<HH", record, by_id[1088000].index(b"\xc7\x00\xc7\x00"), 1, 1)
            ids.append(item_id)
            records.append(record)
            by_id[item_id] = record
        icon_source[item_id] = icon

    twins = 0
    for item_id in sorted(i for i in list(by_id) if i % 10 == 9 and is_equipment(i)):
        twin = display_id(item_id)
        if twin in by_id:
            continue
        template = by_id[item_id]
        name = template[NAME].split(b"\0")[0].decode("latin1")
        record = make_record(template, twin, legendary_name(name), f"{STATS}~{power_text(item_id)}")
        ids.append(twin)
        records.append(record)
        by_id[twin] = record
        icon_source[twin] = item_id
        twins += 1

    write_items(out / "ini" / "itemtype.dat", ids, records)

    for file_name, prefix in (("ItemMinIcon.Ani", "Item"), ("MapItemIcon.Ani", "")):
        text = (client / "ani" / file_name).read_bytes().decode("latin1")
        existing = sections(text)
        additions = [(f"{prefix}{new}", existing[f"{prefix}{old}"]) for new, old in icon_source.items()
                     if f"{prefix}{new}" not in existing and f"{prefix}{old}" in existing]
        (out / "ani" / file_name).write_bytes(append_sections(text, additions).encode("latin1"))

    weapons = (client / "ini" / "weapon.ini").read_bytes().decode("latin1")
    existing = sections(weapons)
    additions = [(str(new), existing[str(old)]) for new, old in icon_source.items()
                 if old // 1000 >= 400 and str(new) not in existing and str(old) in existing]
    (out / "ini" / "weapon.ini").write_bytes(append_sections(weapons, additions).encode("latin1"))

    print(f"Added {twins} Legendary display items, {len(additions)} weapon models.")


if __name__ == "__main__":
    if len(sys.argv) != 3:
        sys.exit(__doc__)
    main(sys.argv[1], sys.argv[2])
