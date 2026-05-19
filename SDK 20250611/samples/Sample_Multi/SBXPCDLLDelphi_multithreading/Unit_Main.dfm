object frmMain: TfrmMain
  Left = 178
  Top = 177
  BorderStyle = bsSingle
  Caption = 'frmMain'
  ClientHeight = 578
  ClientWidth = 989
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 336
    Top = 216
    Width = 133
    Height = 24
    Caption = 'Online Devices:'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object Memo1: TMemo
    Left = 8
    Top = 16
    Width = 961
    Height = 169
    BorderStyle = bsNone
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    Lines.Strings = (
      'SBXPC DLL multi threading sample'
      
        'This program demonstrates how to develop multi-threading program' +
        ' using SBXPC DLL'
      ''
      'Important:'
      '    Every device must have unique device ID'
      '    '
      'Usage:'
      '    1. Add some devices into online list.'
      '    2. Start download 60 fingers.'
      '    3. Stop download when you want.')
    ParentFont = False
    TabOrder = 0
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 232
    Width = 313
    Height = 337
    Caption = 'Manipulation'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    object Label2: TLabel
      Left = 16
      Top = 48
      Width = 85
      Height = 24
      Caption = 'Device ID:'
    end
    object Label3: TLabel
      Left = 16
      Top = 88
      Width = 84
      Height = 24
      Caption = 'Device IP:'
    end
    object Label4: TLabel
      Left = 16
      Top = 120
      Width = 84
      Height = 24
      Caption = 'Port Num:'
    end
    object Label5: TLabel
      Left = 16
      Top = 152
      Width = 87
      Height = 24
      Caption = 'Password:'
    end
    object btnAddDevice: TButton
      Left = 24
      Top = 200
      Width = 121
      Height = 41
      Caption = 'Add'
      TabOrder = 0
      OnClick = btnAddDeviceClick
    end
    object btnRemoveDevice: TButton
      Left = 160
      Top = 200
      Width = 121
      Height = 41
      Caption = 'RemoveAll'
      TabOrder = 1
      OnClick = btnRemoveDeviceClick
    end
    object txtId: TEdit
      Left = 120
      Top = 48
      Width = 145
      Height = 32
      TabOrder = 2
      Text = '1'
    end
    object txtIp: TEdit
      Left = 120
      Top = 80
      Width = 145
      Height = 32
      TabOrder = 3
      Text = '192.168.1.132'
    end
    object txtPort: TEdit
      Left = 120
      Top = 112
      Width = 145
      Height = 32
      TabOrder = 4
      Text = '5005'
    end
    object txtPassword: TEdit
      Left = 120
      Top = 144
      Width = 145
      Height = 32
      TabOrder = 5
      Text = '0'
    end
  end
  object btnStartDownload: TButton
    Left = 496
    Top = 200
    Width = 169
    Height = 33
    Caption = 'Start download'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = btnStartDownloadClick
  end
  object btnStopDownload: TButton
    Left = 672
    Top = 200
    Width = 153
    Height = 33
    Caption = 'Stop Downoad'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -19
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    OnClick = btnStopDownloadClick
  end
  object gridDevices: TStringGrid
    Left = 336
    Top = 240
    Width = 633
    Height = 329
    ColCount = 9
    DefaultRowHeight = 17
    RowCount = 2
    TabOrder = 4
  end
end
