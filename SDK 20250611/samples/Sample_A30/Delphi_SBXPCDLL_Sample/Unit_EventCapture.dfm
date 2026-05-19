object frmEventCapture: TfrmEventCapture
  Left = 265
  Top = 203
  Width = 675
  Height = 640
  Caption = 'frmEventCapture'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object GroupBox3: TGroupBox
    Left = 56
    Top = 32
    Width = 249
    Height = 113
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
    object lblComPort: TLabel
      Left = 24
      Top = 36
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
      Top = 68
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
      Top = 36
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
      Top = 68
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
    Left = 352
    Top = 24
    Width = 249
    Height = 121
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Times New Roman'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
    object lblIPAddress: TLabel
      Left = 31
      Top = 37
      Width = 76
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
      Left = 20
      Top = 68
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
    object optNetworkDevice: TRadioButton
      Left = 8
      Top = 0
      Width = 137
      Height = 17
      Caption = 'Network Device'
      Checked = True
      TabOrder = 0
      TabStop = True
      OnClick = optNetworkDeviceClick
    end
    object txtIPAddress: TEdit
      Left = 112
      Top = 32
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
      Text = '0.0.0.0'
    end
    object txtPortNo: TEdit
      Left = 112
      Top = 64
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
      Text = '5005'
    end
  end
  object cmdStartEventCapture: TButton
    Left = 64
    Top = 160
    Width = 177
    Height = 33
    Caption = 'Start Event Capture'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = cmdStartEventCaptureClick
  end
  object cmdStopEventCapture: TButton
    Left = 248
    Top = 160
    Width = 177
    Height = 33
    Caption = 'Stop Event Capture'
    Enabled = False
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    OnClick = cmdStopEventCaptureClick
  end
  object cmdClear: TButton
    Left = 432
    Top = 160
    Width = 177
    Height = 33
    Caption = 'Clear'
    Font.Charset = ANSI_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'Times New Roman'
    Font.Style = []
    ParentFont = False
    TabOrder = 4
    OnClick = cmdClearClick
  end
  object lstEvent: TListBox
    Left = 16
    Top = 208
    Width = 633
    Height = 377
    ImeName = 'Korean Input System (IME 2000)'
    ItemHeight = 13
    TabOrder = 5
  end
end
