iport sys, re
data = open(r'C:\Program Files (x86)\Access Control System\Korean.XML','rb').read()
m = re.search(b'FrmEmplListInfo_ImageCut', data)
if m:
    chunk = data[m.start()-5:m.start()+800]
    sys.stdout.buffer.write(b'utf8:\n')
    sys.stdout.buffer.write(chunk)
    sys.stdout.buffer.write(b'\n')
