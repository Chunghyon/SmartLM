object frmMain: TfrmMain
  Left = 351
  Top = 205
  Width = 535
  Height = 490
  Caption = 'Main-Delphi'
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
    Width = 33
    Height = 22
    Caption = 'A30'
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
      ItemHeight = 15
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
    Height = 281
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
      Left = 120
      Top = 128
      Width = 113
      Height = 30
      Caption = 'Bell Time'
      TabOrder = 4
      OnClick = cmdBellInfoClick
    end
    object cmdProductCode: TButton
      Left = 8
      Top = 128
      Width = 113
      Height = 30
      Caption = 'Get SN'
      TabOrder = 5
      OnClick = cmdProductCodeClick
    end
    object cmdExit: TButton
      Left = 8
      Top = 240
      Width = 225
      Height = 30
      Caption = 'Exit'
      TabOrder = 6
      OnClick = cmdExitClick
    end
    object cmdHoliday: TButton
      Left = 120
      Top = 160
      Width = 113
      Height = 30
      Caption = 'Holiday'
      TabOrder = 7
      OnClick = cmdHolidayClick
    end
    object cmdModeTZone: TButton
      Left = 8
      Top = 192
      Width = 113
      Height = 30
      Caption = 'ModeTZone'
      TabOrder = 8
      OnClick = cmdModeTZoneClick
    end
    object cmdAccessTz: TButton
      Left = 8
      Top = 160
      Width = 113
      Height = 30
      Caption = 'Access TZone'
      TabOrder = 9
      OnClick = cmdAccessTzClick
    end
    object cmdEventMoniter: TButton
      Left = 120
      Top = 192
      Width = 113
      Height = 30
      Caption = 'Event Monitor'
      TabOrder = 10
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
      ItemHeight = 15
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
      ItemHeight = 15
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
    Height = 129
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
  end
  object optUSBDevice: TRadioButton
    Left = 10
    Top = 408
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
  object SBXPC1: TSBXPC
    Left = 8
    Top = 16
    Width = 113
    Height = 41
    TabOrder = 5
    OnReceiveEventXML = SBXPC1ReceiveEventXML
    ControlData = {00000100AE0B00003D04000000000000}
  end
end
