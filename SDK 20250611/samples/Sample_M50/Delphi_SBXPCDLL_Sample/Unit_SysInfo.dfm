object frmSystemInfo: TfrmSystemInfo
  Left = 491
  Top = 397
  Caption = 'System Infomation'
  ClientHeight = 269
  ClientWidth = 446
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 24
    Top = 168
    Width = 110
    Height = 19
    Caption = 'Status Parameter :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 24
    Top = 184
    Width = 97
    Height = 19
    Caption = 'Info Parameter :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 272
    Top = 176
    Width = 81
    Height = 19
    Caption = 'Status Value :'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
  end
  object lblMessage: TStaticText
    Left = 8
    Top = 16
    Width = 433
    Height = 33
    Align = alCustom
    Alignment = taCenter
    AutoSize = False
    BorderStyle = sbsSunken
    Caption = 'Message'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -17
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
  end
  object cmdGetDeviceTime: TButton
    Left = 8
    Top = 72
    Width = 129
    Height = 32
    Caption = 'GetDeviceTime'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    OnClick = cmdGetDeviceTimeClick
  end
  object cmdPowerOn: TButton
    Left = 152
    Top = 72
    Width = 129
    Height = 32
    Caption = 'PowerOnDevice'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 2
    OnClick = cmdPowerOnClick
  end
  object cmdEnableDevice: TButton
    Left = 304
    Top = 72
    Width = 137
    Height = 32
    Caption = 'DisableDevice'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 3
    OnClick = cmdEnableDeviceClick
  end
  object cmdSetDeviceTime: TButton
    Left = 8
    Top = 120
    Width = 129
    Height = 32
    Caption = 'SetDeviceTime'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 4
    OnClick = cmdSetDeviceTimeClick
  end
  object PowerOffDevice: TButton
    Left = 152
    Top = 120
    Width = 129
    Height = 32
    Caption = 'PowerOffDevice'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 5
    OnClick = PowerOffDeviceClick
  end
  object cmdExit: TButton
    Left = 304
    Top = 120
    Width = 137
    Height = 32
    Caption = 'Exit'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 6
    OnClick = cmdExitClick
  end
  object cmbSatus: TComboBox
    Left = 152
    Top = 176
    Width = 89
    Height = 24
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 7
    Items.Strings = (
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '9'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '17'
      '18'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24')
  end
  object txtSetDevInfo: TEdit
    Left = 368
    Top = 176
    Width = 57
    Height = 24
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ImeName = 'Korean Input System (IME 2000)'
    ParentFont = False
    TabOrder = 8
    Text = '0'
  end
  object cmdGetDeviceInfo: TButton
    Left = 8
    Top = 216
    Width = 129
    Height = 33
    Caption = 'GetDeviceInfo'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 9
    OnClick = cmdGetDeviceInfoClick
  end
  object cmdSetDeviceInfo: TButton
    Left = 152
    Top = 216
    Width = 129
    Height = 33
    Caption = 'SetDeviceInfo'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 10
    OnClick = cmdSetDeviceInfoClick
  end
  object cmdGetDeviceStaus: TButton
    Left = 304
    Top = 216
    Width = 137
    Height = 33
    Caption = 'GetDeviceStatus'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 11
    OnClick = cmdGetDeviceStausClick
  end
  object chkEnableDevice: TCheckBox
    Left = 284
    Top = 80
    Width = 17
    Height = 17
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 12
  end
end
