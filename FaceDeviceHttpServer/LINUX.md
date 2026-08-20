# Linux에서 FDHS 실행

## 게시

```bash
cd FaceDeviceHttpServer
./publish-linux.sh
```

또는:

```bash
dotnet publish FaceDeviceHttpServer.csproj -c Release -f net9.0 -r linux-x64 --self-contained true -o dist/fdhs-linux
```

Windows Visual Studio에서는 `net9.0-windows`로 기존처럼 WinForms 로그 창이 뜹니다.

## 실행

```bash
sudo mkdir -p /opt/smartlm
sudo cp -r dist/fdhs-linux/* /opt/smartlm/
cd /opt/smartlm
sudo ./FaceDeviceHttpServer
```

80 포트는 root 또는:

```bash
sudo setcap cap_net_bind_service=+ep /opt/smartlm/FaceDeviceHttpServer
```

## systemd

```bash
sudo cp smartlm-fdhs.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable --now smartlm-fdhs
```

데이터/설정 기본 위치: `~/SmartLM_Data/` (`{MyDocuments}`)
