import sys, re
sys.stdout.reconfigure(encoding='utf-8')

data = open(r'C:\Program Files (x86)\Access Control System\Card3500.exe','rb').read()

# EUC-KR '����' = c0fac0e5
pattern = b'\xc0\xfa\xc0\xe5'
found = []
idx = 0
while True:
    idx = data.find(pattern, idx)
    if idx == -1: break
    context = data[max(0,idx-30):idx+50]
    found.append((idx, context))
    idx += 1

print(f'���� ���� �߰� Ƚ��: {len(found)}')
for pos, ctx in found[:15]:
    try:
        decoded = ctx.decode('euc-kr', 'replace')
        print(f'  pos={pos}: {decoded!r}')
    except:
        pass
