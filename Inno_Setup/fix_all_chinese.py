# -*- coding: utf-8 -*-
"""
1. Korean.XML (��ġ�� + Inno_Setup) - �߱��� �±װ� ����
2. SoftWareInfo_Korean.XML (��ġ�� + Inno_Setup) - EquptTypeInfo ����
3. Language\ICCardEditer\Korean.xml ����
4. ICCard_Editer.exe.config - Korean ��� �߰� �� �⺻�� ����
"""

import os, re, shutil

APP = r'C:\Program Files (x86)\Access Control System'
INNO = r'D:\Documents\Smart_LM_China\Inno_Setup'

# ��������������������������������������������������������������������������������������������������������������������������
# 1. Korean.XML �߱��� �±װ� ���� (�̹��� �ڸ��� ���� �� �˾� ����)
# ��������������������������������������������������������������������������������������������������������������������������
korean_fixes = [
    # RecordSignature (���� ����)
    ('<RecordSignature>\u7b7e\u540d</RecordSignature>',         '<RecordSignature>����</RecordSignature>'),
    ('<RecordSignature_0>\u672a\u7b7e\u540d</RecordSignature_0>','<RecordSignature_0>�̼���</RecordSignature_0>'),
    ('<RecordSignature_1>\u5df2\u7b7e\u540d</RecordSignature_1>','<RecordSignature_1>�����</RecordSignature_1>'),
    ('<RecordSignature_2>\u5df2\u9a8c\u8bc1</RecordSignature_2>','<RecordSignature_2>������</RecordSignature_2>'),
    ('<RecordSignature_3>\u9a8c\u8bc1\u9519\u8bef</RecordSignature_3>','<RecordSignature_3>���� ����</RecordSignature_3>'),
    # ���� ��� �����
    ('<PrintRecord>\u6253\u5370\u51fa\u5165\u8bb0\u5f55\u62a5\u8868</PrintRecord>',
     '<PrintRecord>���� ��� ����� �μ�</PrintRecord>'),
    ('<OutputRecord>\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u62a5\u8868</OutputRecord>',
     '<OutputRecord>���� ��� ����� ��������</OutputRecord>'),
    ('<OutputTotalRecord>\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u7edf\u8ba1\u62a5\u8868</OutputTotalRecord>',
     '<OutputTotalRecord>���� ��� ��� ����� ��������</OutputTotalRecord>'),
    ('<PrintFaceRecord>\u6253\u5370\u51fa\u5165\u8bb0\u5f55\u7167\u7247\u62a5\u8868</PrintFaceRecord>',
     '<PrintFaceRecord>���� ��� ���� ����� �μ�</PrintFaceRecord>'),
    ('<OutputFaceRecord>\u5bfc\u51fa\u51fa\u5165\u8bb0\u5f55\u7167\u7247\u62a5\u8868</OutputFaceRecord>',
     '<OutputFaceRecord>���� ��� ���� ����� ��������</OutputFaceRecord>'),
    ('<SearchRecordLog>\u67e5\u8be2\u51fa\u5165\u8bb0\u5f55\u62a5\u8868</SearchRecordLog>',
     '<SearchRecordLog>���� ��� ����� ��ȸ</SearchRecordLog>'),
    # �̺�Ʈ
    ('<lblEventCode>\u4e8b\u4ef6\uff1a</lblEventCode>',
     '<lblEventCode>�̺�Ʈ:</lblEventCode>'),
    ('<cmbEventCodeToolTip>\u652f\u6301\u591a\u6761\u4ef6\u8f93\u5165\uff0c\u4f7f\u7528\u9017\u53f7\u5206\u9694\uff0c\u793a\u4f8b\uff1a \u5237\u5361\u9a8c\u8bc1,\u4eba\u8138\u9a8c\u8bc1</cmbEventCodeToolTip>',
     '<cmbEventCodeToolTip>���� ���� �Է� ����, ��ǥ�� ����. ��: ī�� ����, �� ����</cmbEventCodeToolTip>'),
    # ���� �׷� �α�
    ('<UpdateEmplAccessGroupLog>\u4fee\u6539\u4eba\u5458\u6743\u9650\u7ec4: %PName (%PCode)\uff1b \u65b0\u6743\u9650\u7ec4:%Group.</UpdateEmplAccessGroupLog>',
     '<UpdateEmplAccessGroupLog>����� ���� �׷� ����: %PName (%PCode); �� �׷�: %Group.</UpdateEmplAccessGroupLog>'),
    ('<DeleteEmplAccessGroupLog>\u5220\u9664\u4eba\u5458\u6743\u9650\u7ec4: %PName (%PCode)\uff1b\u6240\u5c5e\u6743\u9650\u7ec4:%Group.</DeleteEmplAccessGroupLog>',
     '<DeleteEmplAccessGroupLog>����� ���� �׷� ����: %PName (%PCode); �Ҽ� �׷�: %Group.</DeleteEmplAccessGroupLog>'),
]

for fpath in [os.path.join(APP, 'Korean.XML'), os.path.join(INNO, 'Korean.XML')]:
    content = open(fpath, encoding='utf-8').read()
    count = 0
    for src, dst in korean_fixes:
        if src in content:
            content = content.replace(src, dst)
            count += 1
    open(fpath, 'w', encoding='utf-8').write(content)
    print(f'Korean.XML ({os.path.dirname(fpath).split(chr(92))[-1]}): {count}�� ����')

# ���� �� �߱��� ���� �±װ� Ȯ��
for fpath in [os.path.join(APP, 'Korean.XML'), os.path.join(INNO, 'Korean.XML')]:
    content = open(fpath, encoding='utf-8').read()
    hits = [l.strip() for l in content.split('\n')
            if re.search(r'[\u4e00-\u9fff]{2,}', l) and not l.strip().startswith('<!--')]
    if hits:
        print(f'  ? ���� �߱��� ({fpath}): {len(hits)}��')
        for h in hits: print(f'    {h[:80]}')
    else:
        print(f'  ? �߱��� �±װ� ����')

# ��������������������������������������������������������������������������������������������������������������������������
# 2. SoftWareInfo_Korean.XML - EquptTypeInfo ����
# ��������������������������������������������������������������������������������������������������������������������������
equpt_fixes = [
    ('<EquptTypeInfo_E32>32\u8def%1\u63a7\u5236\u5668\uff0c\u53ef\u4ee5\u652f\u6301322%2\uff0c\u4e5f\u53ef\u4ee5\u901a\u8fc7\u589e\u52a01\u4e2a\u6269\u5c55\u677f\u652f\u6301\u8fbe\u523064%2\u3002\u4e3b\u677f\u652f\u63012600\u52700\u7528\u6237\u4e0e10\u4e07\u7b14\u8bb0\u5f55\u3002</EquptTypeInfo_E32>',
     '<EquptTypeInfo_E32>32ä�� %1 ��Ʈ�ѷ�. 32�� %2 ����, Ȯ�� ���� �߰� �� 64�� %2���� Ȯ�� ����. ���κ��� �ִ� 26,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_E32>'),
    ('<EquptTypeInfo_E16>16\u8def%1\u63a7\u5236\u5668\uff0c\u53ef\u4ee5\u652f\u6301162%2\uff0c\u4e5f\u53ef\u4ee5\u901a\u8fc7\u589e\u52a01\u4e2a\u6269\u5c55\u677f\u652f\u6301\u8fbe\u523032%2\u3002\u4e3b\u677f\u652f\u63012600\u52700\u7528\u6237\u4e0e10\u4e07\u7b14\u8bb0\u5f55\u3002</EquptTypeInfo_E16>',
     '<EquptTypeInfo_E16>16ä�� %1 ��Ʈ�ѷ�. 16�� %2 ����, Ȯ�� ���� �߰� �� 32�� %2���� Ȯ�� ����. ���κ��� �ִ� 26,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_E16>'),
    ('<EquptTypeInfo_OE8>\u8131\u673a8\u8def%1\u63a7\u5236\u5668\uff0c\u53ef\u4ee5\u652f\u63018%2\uff0c\u4e5f\u53ef\u4ee5\u901a\u8fc7\u589e\u52a01\u4e2a\u6269\u5c55\u677f\u652f\u6301\u8fbe\u523016%2\u3002\u4e3b\u677f\u652f\u63019000\u52700\u7528\u6237\u4e0e10\u4e07\u7b14\u8bb0\u5f55\u3002</EquptTypeInfo_OE8>',
     '<EquptTypeInfo_OE8>�������� 8ä�� %1 ��Ʈ�ѷ�. 8�� %2 ����, Ȯ�� ���� �߰� �� 16�� %2���� Ȯ�� ����. ���κ��� �ִ� 90,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_OE8>'),
    ('<EquptTypeInfo_OE16>\u8131\u673a16\u8def%1\u63a7\u5236\u5668\uff0c\u53ef\u4ee5\u652f\u63016162%2\u3002\u4e3b\u677f\u652f\u63019000\u52700\u7528\u6237\u4e0e10\u4e07\u7b14\u8bb0\u5f55\u3002</EquptTypeInfo_OE16>',
     '<EquptTypeInfo_OE16>�������� 16ä�� %1 ��Ʈ�ѷ�. 16�� %2 ����. ���κ��� �ִ� 90,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_OE16>'),
]

# ���� ���Ͽ��� �� Ȯ�� �� ���� ��ü
for fpath in [os.path.join(APP, 'SoftWareInfo_Korean.XML'), os.path.join(INNO, 'SoftWareInfo_Korean.XML')]:
    content = open(fpath, encoding='utf-8').read()
    # ���Խ����� EquptTypeInfo �±� ��ü ��ü
    replacements = [
        (r'<EquptTypeInfo_E32>[^<]*</EquptTypeInfo_E32>',
         '<EquptTypeInfo_E32>32ä�� %1 ��Ʈ�ѷ�. 32�� %2 ����, Ȯ�� ���� �߰� �� 64�� %2���� Ȯ�� ����. ���κ��� �ִ� 26,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_E32>'),
        (r'<EquptTypeInfo_E16>[^<]*</EquptTypeInfo_E16>',
         '<EquptTypeInfo_E16>16ä�� %1 ��Ʈ�ѷ�. 16�� %2 ����, Ȯ�� ���� �߰� �� 32�� %2���� Ȯ�� ����. ���κ��� �ִ� 26,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_E16>'),
        (r'<EquptTypeInfo_OE8>[^<]*</EquptTypeInfo_OE8>',
         '<EquptTypeInfo_OE8>�������� 8ä�� %1 ��Ʈ�ѷ�. 8�� %2 ����, Ȯ�� ���� �߰� �� 16�� %2���� Ȯ�� ����. ���κ��� �ִ� 90,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_OE8>'),
        (r'<EquptTypeInfo_OE16>[^<]*</EquptTypeInfo_OE16>',
         '<EquptTypeInfo_OE16>�������� 16ä�� %1 ��Ʈ�ѷ�. 16�� %2 ����. ���κ��� �ִ� 90,000�� �����, 100,000�� ��� ����.</EquptTypeInfo_OE16>'),
    ]
    count = 0
    for pattern, repl in replacements:
        new_content, n = re.subn(pattern, repl, content)
        content = new_content
        count += n
    open(fpath, 'w', encoding='utf-8').write(content)
    print(f'SoftWareInfo_Korean.XML ({os.path.dirname(fpath).split(chr(92))[-1]}): {count}�� ����')

# ��������������������������������������������������������������������������������������������������������������������������
# 3. ICCardEditer Korean.xml ����
# ��������������������������������������������������������������������������������������������������������������������������
korean_iccard = '''<?xml version="1.0" encoding="utf-8" ?>
<Language>
  <FrmLogin>
    <FormCaption>ICī��/CPUī�� Ű ������ ver1.9</FormCaption>
    <Label4>ICī��/CPUī�� Ű ������</Label4>
    <Label1>����ڸ��</Label1>
    <Label2>��й�ȣ��</Label2>
    <ButLogin>�α���</ButLogin>
    <ButExit>����</ButExit>
    <MsgBox1>�����ͺ��̽� ���� ��, ��� ��ٷ� �ּ���....</MsgBox1>
    <MsgBox2>������ ������ �����ϴ�</MsgBox2>
    <MsgBox3>�����ͺ��̽� ���� ����. ��й�ȣ�� �Է��ϼ���!</MsgBox3>
    <MsgBox4>�����ͺ��̽� ���� ����!</MsgBox4>
    <MsgBox5>ī�� ����Ʈ��� ���� �����ͺ��̽� ���� �Ķ���͸� Ȯ���� �ּ���!</MsgBox5>
    <MsgBox6>������ ��й�ȣ�� �Է��ϼ���!</MsgBox6>
    <MsgBox7>������ ��й�ȣ ����!</MsgBox7>
  </FrmLogin>
  <FrmEdit>
    <FormCaption>ICī��/CPUī�� Ű ������ ver1.9</FormCaption>
    <stbICPassword>ī�� ��й�ȣ ǥ</stbICPassword>
    <stbEncrypt>��ȣȭ/��ȣȭ</stbEncrypt>
    <SuperTabItem1>ī�� ������ ����</SuperTabItem1>
    <LabelX3>ī�� �߱� �Ǵ� �Һ� ����Ʈ���� �߱� �Ŀ��� ������ �� �����ϴ�</LabelX3>
    <butSave>����</butSave>
    <butExit>����</butExit>
    <LblSecretkey>2�� ��ȣȭ ��й�ȣ (��ȣȭ�� ���):</LblSecretkey>
    <LabelX1>ī�� �߱ޱ� ������</LabelX1>
    <LabelX2>���£�</LabelX2>
    <LabelX5>��ȣȭ Ƚ����</LabelX5>
    <ButEncode>��ȣȭ</ButEncode>
    <butDecode>��ȣȭ</butDecode>
    <butExit1>����</butExit1>
    <butStop>����</butStop>
    <Label15>ī�� ������ ����� �����ϴ� ī�� ������</Label15>
    <Label7>��Ʈ�� ī�� ��� ����ġ��</Label7>
    <btnWriteConfigCard>��Ʈ�� ī�� ��� ����ġ ���� ī�� �����</btnWriteConfigCard>
    <NotReader>ī�� �����⸦ ã�� �� �����ϴ�</NotReader>
    <ParReader>�Ķ���� ����</ParReader>
    <Password>��й�ȣ ����</Password>
    <NotCard>ī�尡 �����ϴ�</NotCard>
    <DataErr>������ ��� ũ�� ����</DataErr>
    <Sections>����,��й�ȣ,���,�뵵</Sections>
    <ReaderTypes>MF1 ICī��,NFCī��,NFC��,�ź���,CPU ICī��,CPUī��,IDī��</ReaderTypes>
    <SectionTypes0>�̻��</SectionTypes0>
    <SectionTypes1>���� ī�� ��ȣ</SectionTypes1>
    <SectionTypes2>Wiegand ������</SectionTypes2>
    <SectionTypes3>���� �ý���</SectionTypes3>
    <SectionTypes4>����Ʈ ���°�</SectionTypes4>
    <SectionTypes5>���� �ý���1</SectionTypes5>
    <SectionTypes6>���� �ý���2</SectionTypes6>
    <SectionTypes7>�������� ��������/������ �ý���</SectionTypes7>
    <SectionTypes8>�������� ���������� ����</SectionTypes8>
    <SectionTypes9>�Һ� �ý���1</SectionTypes9>
    <SectionTypes10>�Һ� �ý���2</SectionTypes10>
    <SectionTypes11>�Һ� �ý���3</SectionTypes11>
    <SectionTypes12>�� ���� �ý���</SectionTypes12>
    <SectionTypes13>ȣ�� ���/������ ���</SectionTypes13>
    <SectionTypes14>����� �ý���</SectionTypes14>
    <SectionTypes15>������ �ý���</SectionTypes15>
    <SectionTypes16>�ý��� �׽�Ʈ</SectionTypes16>
    <MsgBox1>���� ��ȣȭ/��ȣȭ�� �����ϼ���!</MsgBox1>
    <MsgBox2>��й�ȣ ǥ�� �����Ǿ����� ������� �ʾҽ��ϴ�. ���� �����Ͻðڽ��ϱ�?</MsgBox2>
    <MsgBox3>Ȯ��</MsgBox3>
    <MsgBox4>�ý��� ��ȣȭ ������ �����ϴ�!</MsgBox4>
    <MsgBox5>����</MsgBox5>
    <MsgBox6>���� ��й�ȣ�� 12�ڸ����� �մϴ�. ��� ���� ����: 1234567890ABCDEF��</MsgBox6>
    <MsgBox7>�˸�</MsgBox7>
    <MsgBox8>���� ��й�ȣ�� 12�ڸ����� �մϴ٣�</MsgBox8>
    <MsgBox9>���� �Ϸᣡ</MsgBox9>
    <MsgBox10>��й�ȣ ǥ�� ���� {0} ��й�ȣ�� �ùٸ��� �ʽ��ϴ�. 12�ڸ� ��й�ȣ�� �Է��ϼ���!</MsgBox10>
    <MsgBox11>��й�ȣ ǥ�� ���� {0} ��� �뵵�� �ٸ� ���Ϳ� �ߺ��˴ϴ�. ������ �ּ���!</MsgBox11>
    <MsgBox12>ī�� �����⸦ ã�� �� �����ϴ�</MsgBox12>
    <MsgBox13>ī�� ������, {0} ó�� ��...</MsgBox13>
    <MsgBox14>�������� �ʴ� �����Դϴ� --</MsgBox14>
    <MsgBox15>ī�尡 �������� �ʽ��ϴ�</MsgBox15>
    <MsgBox16>ī�� ������ Ȯ�� ��....</MsgBox16>
    <MsgBox17>��ȣȭ ����</MsgBox17>
    <MsgBox18>��ȣȭ ����</MsgBox18>
    <MsgBox19>��ȣȭ ����</MsgBox19>
    <MsgBox20>��ȣȭ ����</MsgBox20>
    <MsgBox21>ī�� ������ ����</MsgBox21>
    <MsgBox22>ī�� ������, ���� ���� ��...</MsgBox22>
    <MsgBox23>���� ���� �Ϸᣡ</MsgBox23>
    <MsgBox24>���� ���� ����. ī�带 ��ü�� �ּ��䣡</MsgBox24>
  </FrmEdit>
</Language>
'''

iccard_korean_path = os.path.join(APP, 'Language', 'ICCardEditer', 'Korean.xml')
open(iccard_korean_path, 'w', encoding='utf-8').write(korean_iccard)
print(f'Korean.xml ���� �Ϸ�: {iccard_korean_path}')

# Inno_Setup���� ������ �� �ֵ��� ��� ����
inno_iccard_dir = os.path.join(INNO, 'Language', 'ICCardEditer')
os.makedirs(inno_iccard_dir, exist_ok=True)
shutil.copy(iccard_korean_path, os.path.join(inno_iccard_dir, 'Korean.xml'))
print(f'Korean.xml Inno_Setup ���� �Ϸ�')

# ��������������������������������������������������������������������������������������������������������������������������
# 4. ICCard_Editer.exe.config - Korean ��� �߰� �� �⺻�� ����
# ��������������������������������������������������������������������������������������������������������������������������
config_path = os.path.join(APP, 'ICCard_Editer.exe.config')
config = open(config_path, encoding='utf-8').read()

config = config.replace(
    '<add key="Languages" value="??,��?,English" />',
    '<add key="Languages" value="Korean,English,??,��?" />'
).replace(
    '<add key="Language_English" value="Language\\ICCardEditer\\English.xml" />',
    '<add key="Language_Korean" value="Language\\ICCardEditer\\Korean.xml" />\n\t\t<add key="Language_English" value="Language\\ICCardEditer\\English.xml" />'
).replace(
    '<add key="ToolLanguage" value="English" />',
    '<add key="ToolLanguage" value="Korean" />'
)

open(config_path, 'w', encoding='utf-8').write(config)
print(f'ICCard_Editer.exe.config ���� �Ϸ�')

print('\n=== ��� �۾� �Ϸ� ===')
