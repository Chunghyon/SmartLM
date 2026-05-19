object frmMain: TfrmMain
  Left = 909
  Top = 198
  Caption = 'Main-Delphi'
  ClientHeight = 482
  ClientWidth = 530
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 160
    Top = 24
    Width = 182
    Height = 31
    Caption = 'SBXPC Sample'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clRed
    Font.Height = -27
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Label2: TLabel
    Left = 224
    Top = 56
    Width = 64
    Height = 22
    Caption = 'SB3600'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clBlue
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold, fsItalic]
    ParentFont = False
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 88
    Width = 497
    Height = 65
    Caption = ' Connect '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
    object Label3: TLabel
      Left = 24
      Top = 24
      Width = 117
      Height = 19
      Caption = 'Machine Number : '
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object cmbMachineNumber: TComboBox
      Left = 152
      Top = 24
      Width = 89
      Height = 23
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 0
      Items.Strings = (
        '1'
        '2'
        '3'
        '4'
        '5'
        '6'
        '7'
        '8'
        '9')
    end
    object cmdOpen: TButton
      Left = 264
      Top = 22
      Width = 113
      Height = 27
      Caption = 'Open'
      TabOrder = 1
      OnClick = cmdOpenClick
    end
    object cmdClose: TButton
      Left = 384
      Top = 22
      Width = 105
      Height = 27
      Caption = 'Close'
      TabOrder = 2
      OnClick = cmdCloseClick
    end
  end
  object GroupBox2: TGroupBox
    Left = 264
    Top = 160
    Width = 241
    Height = 305
    Caption = ' Management '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    object cmdEnrollData: TButton
      Left = 8
      Top = 32
      Width = 225
      Height = 30
      Caption = 'Enroll Data Management'
      TabOrder = 0
      OnClick = cmdEnrollDataClick
    end
    object cmdLogData: TButton
      Left = 8
      Top = 64
      Width = 225
      Height = 30
      Caption = 'Log Data Management'
      TabOrder = 1
      OnClick = cmdLogDataClick
    end
    object cmdSystemInfo: TButton
      Left = 8
      Top = 96
      Width = 113
      Height = 30
      Caption = 'System Info'
      TabOrder = 2
      OnClick = cmdSystemInfoClick
    end
    object cmdLockCtl: TButton
      Left = 120
      Top = 96
      Width = 113
      Height = 30
      Caption = 'Lock Control'
      TabOrder = 3
      OnClick = cmdLockCtlClick
    end
    object cmdBellInfo: TButton
      Left = 8
      Top = 128
      Width = 113
      Height = 30
      Caption = 'Bell Time'
      TabOrder = 4
      OnClick = cmdBellInfoClick
    end
    object cmdProductCode: TButton
      Left = 8
      Top = 160
      Width = 113
      Height = 30
      Caption = 'Get SN'
      TabOrder = 5
      OnClick = cmdProductCodeClick
    end
    object cmdExit: TButton
      Left = 8
      Top = 264
      Width = 225
      Height = 30
      Caption = 'Exit'
      TabOrder = 6
      OnClick = cmdExitClick
    end
    object cmdDepartment: TButton
      Left = 8
      Top = 192
      Width = 113
      Height = 30
      Caption = 'Department'
      TabOrder = 7
      OnClick = cmdDepartmentClick
    end
    object cmdScreenSaver: TButton
      Left = 8
      Top = 224
      Width = 113
      Height = 30
      Caption = 'Screen Saver'
      TabOrder = 8
      OnClick = cmdScreenSaverClick
    end
    object cmdTrMode: TButton
      Left = 120
      Top = 128
      Width = 113
      Height = 30
      Caption = 'Log TZone'
      TabOrder = 9
      OnClick = cmdTrModeClick
    end
    object cmdDaigong: TButton
      Left = 120
      Top = 192
      Width = 113
      Height = 30
      Caption = 'Daigong'
      TabOrder = 10
      OnClick = cmdDaigongClick
    end
    object cmdAccessTz: TButton
      Left = 120
      Top = 160
      Width = 113
      Height = 30
      Caption = 'Access TZone'
      TabOrder = 11
      OnClick = cmdAccessTzClick
    end
    object cmdEventMoniter: TButton
      Left = 120
      Top = 224
      Width = 113
      Height = 30
      Caption = 'Event Monitor'
      TabOrder = 12
      OnClick = cmdEventMoniterClick
    end
  end
  object GroupBox3: TGroupBox
    Left = 8
    Top = 160
    Width = 249
    Height = 97
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    object lblComPort: TLabel
      Left = 24
      Top = 28
      Width = 63
      Height = 19
      Caption = 'ComPort :'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object lblBaudrate: TLabel
      Left = 24
      Top = 60
      Width = 62
      Height = 19
      Caption = 'Baudrate :'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object cmbComPort: TComboBox
      Left = 104
      Top = 28
      Width = 121
      Height = 23
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 0
      Items.Strings = (
        '1'
        '2'
        '3'
        '4'
        '5'
        '6'
        '7'
        '8')
    end
    object cmbBaudrate: TComboBox
      Left = 104
      Top = 60
      Width = 121
      Height = 23
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -13
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 1
      Items.Strings = (
        '9600'
        '19200'
        '38400'
        '57600'
        '115200')
    end
    object optSerialDevice: TRadioButton
      Left = 8
      Top = 0
      Width = 121
      Height = 17
      Caption = 'Serial Device'
      TabOrder = 2
      OnClick = optSerialDeviceClick
    end
  end
  object GroupBox4: TGroupBox
    Left = 8
    Top = 264
    Width = 249
    Height = 185
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    object lblIPAddress: TLabel
      Left = 23
      Top = 29
      Width = 74
      Height = 19
      Caption = 'IP Address :'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object lblPortNo: TLabel
      Left = 12
      Top = 60
      Width = 87
      Height = 19
      Caption = 'Port Number :'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object lblPassword: TLabel
      Left = 32
      Top = 88
      Width = 67
      Height = 19
      Caption = 'Password :'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object Label4: TLabel
      Left = 16
      Top = 120
      Width = 116
      Height = 19
      Caption = 'Device Unique ID :'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ParentFont = False
    end
    object optNetworkDevice: TRadioButton
      Left = 8
      Top = 0
      Width = 137
      Height = 17
      Caption = 'Network Device'
      TabOrder = 0
      OnClick = optNetworkDeviceClick
    end
    object txtIPAddress: TEdit
      Left = 104
      Top = 24
      Width = 121
      Height = 27
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 1
      Text = '192.168.1.200'
    end
    object txtPortNo: TEdit
      Left = 104
      Top = 56
      Width = 121
      Height = 27
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 2
      Text = '4000'
    end
    object txtPassword: TEdit
      Left = 104
      Top = 88
      Width = 121
      Height = 27
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 3
      Text = '0'
    end
    object txtDeviceUuid: TEdit
      Left = 16
      Top = 144
      Width = 209
      Height = 27
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -16
      Font.Name = 'Times New Roman'
      Font.Style = []
      ImeName = 'Korean Input System (IME 2000)'
      ParentFont = False
      TabOrder = 4
      Text = '1466F1A61E5D1010'
    end
  end
  object optUSBDevice: TRadioButton
    Left = 18
    Top = 464
    Width = 113
    Height = 17
    Caption = 'USB Device'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = optUSBDeviceClick
  end
end
