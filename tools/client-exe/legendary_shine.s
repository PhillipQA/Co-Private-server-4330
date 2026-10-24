.intel_syntax noprefix
.code32
# eax = item type id. Returns eax with Legendary display ids mapped back to their Super id
# (so the client draws the Super shine), plus edx = 0 and ecx = 10 as the replaced code expected.
q:
    cmp eax, 410300
    jne 1f
    add eax, 9
    jmp done
1:  cmp eax, 421302
    jne 2f
    add eax, 7
    jmp done
2:  cmp eax, 500302
    jne 3f
    add eax, 7
    jmp done
3:  push eax
    xor edx, edx
    mov ecx, 10
    div ecx
    cmp edx, 1
    jne keep
    xor edx, edx
    mov ecx, 100
    div ecx
    cmp eax, 111
    jb keep
    cmp eax, 160
    jbe equip
    cmp eax, 400
    jb keep
    cmp eax, 599
    jbe equip
    cmp eax, 900
    jne keep
equip:
    mov eax, [esp]
    cmp eax, 410301
    je keep
    cmp eax, 410401
    je keep
    cmp eax, 410501
    je keep
    cmp eax, 410601
    je keep
    cmp eax, 410701
    je keep
    cmp eax, 410801
    je keep
    cmp eax, 410901
    je keep
    cmp eax, 421301
    je keep
    cmp eax, 500301
    je keep
    cmp eax, 562001
    je keep
    pop eax
    add eax, 8
    jmp done
keep:
    pop eax
done:
    xor edx, edx
    mov ecx, 10
    ret
