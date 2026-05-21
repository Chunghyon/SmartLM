# coding: utf-8
import sys, re
sys.stdout.reconfigure(encoding='utf-8')

data = open(r'C:\Users\Public\Card3500_unpacked.exe', 'rb').read()
latin1 = data.decode('latin1')

# PhotoDemo ��ġ��
positions = []
pos = 0
while True:
    idx = latin1.find('PhotoDemo', pos)
    if idx < 0: break
    positions.append(idx)
    pos = idx + 1

print(f'PhotoDemo positions: {positions}')

# 2077860, 3876972 �� �ٸ� ��ġ��
for ppos in positions:
    if ppos not in (2077860, 3876972):
        window = latin1[max(0,ppos-200):ppos+300]
        out = ''.join(c if 32 <= ord(c) <= 126 else '.' for c in window)
        print(f'OTHER pos={ppos}: {out[:200]}')
        print()
