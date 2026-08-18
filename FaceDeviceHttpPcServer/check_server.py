import urllib.request, json

url = 'http://localhost:8100/api/Attendance/Search'
body = json.dumps({"PageIndex":1,"PageSize":5}).encode('utf-8')
req = urllib.request.Request(url, data=body, headers={'Content-Type':'application/json'}, method='POST')
try:
    with urllib.request.urlopen(req, timeout=5) as resp:
        print("STATUS:", resp.status)
        print(resp.read().decode('utf-8','replace')[:500])
except urllib.error.HTTPError as e:
    print("HTTP ERROR:", e.code, e.read().decode('utf-8','replace')[:500])
except Exception as e:
    print("ERROR:", e)
