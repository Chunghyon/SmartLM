package smack.comm.sample;

import java.awt.EventQueue;
import java.awt.Font;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.IOException;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JTextField;
import javax.swing.SwingConstants;

import com.sun.media.jfxmedia.track.Track.Encoding;

import smack.comm.SBXPCProxy;
import smack.comm.output.OneStringOutput;
import smack.comm.sample.global.SysUtil;
import sun.misc.BASE64Decoder;
import sun.nio.cs.UnicodeEncoder;

import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import javax.swing.JPanel;
import javax.swing.border.TitledBorder;
import javax.swing.JCheckBox;
import javax.swing.UIManager;
import java.awt.Color;

public class NetworkSettingFrame extends JFrame {
	private JLabel lblSubnet;
	private JLabel lblGateway;
	private JTextField txtEther_Subnet;
	private JTextField txtEther_DefaultGateway;
	private JButton btnGetEthernetSetting;
	private JButton btnSetEthernetSetting;
	private JCheckBox chkEther_DHCP;
	private JLabel lblIp;
	private JTextField txtEther_IP;
	private JLabel label_1;
	private JTextField txtEther_PrimaryDNSServer;
	private JLabel lblSecondaryDnsServer;
	private JTextField txtEther_SecondaryDNSServer;
	private JCheckBox chkEther_ManualDNS;
	private JPanel panel_1;
	private JCheckBox chkWiFi_DHCP;
	private JLabel lblSubnetMask;
	private JLabel lblDefaultGateway;
	private JTextField txtWiFi_Subnet;
	private JTextField txtWiFi_DefaultGateway;
	private JButton btnGetWiFiSetting;
	private JButton btnSetWiFiSetting;
	private JLabel lblIp_1;
	private JTextField txtWiFi_IP;
	private JLabel label_6;
	private JTextField txtWiFi_PrimaryDNSServer;
	private JLabel lblSecondaryDnsServer_1;
	private JTextField txtWiFi_SecondaryDNSServer;
	private JCheckBox chkWiFi_ManualDNS;
	private JTextField txtWiFi_SSID;
	private JLabel lblSsid;
	private JLabel lblKey;
	private JTextField txtWiFi_Key;
	private JPanel panel_2;
	private JLabel lblCommunicationPassword;
	private JLabel lblTcpPort;
	private JTextField txtCommPwd;
	private JTextField txtTcpPort;
	private JButton btnGetCommSetting;
	private JButton btnSetCommSetting;
	private JLabel lblDeviceid;
	private JTextField txtDeviceID;
	private JLabel lblPpServerIp;
	private JTextField txtP2P_Server;
	private JLabel lblPpServerPort;
	private JTextField txtP2P_Port;
	private JButton btnApplyCommSetting;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					NetworkSettingFrame frame = new NetworkSettingFrame();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 */
	public NetworkSettingFrame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});
	
		setTitle("NetworkSetting (for M90 Rev1)");
		setBounds(100, 100, 715, 530);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);
		
		JPanel panel = new JPanel();
		panel.setBorder(new TitledBorder(null, "Ethernet Setting", TitledBorder.LEADING, TitledBorder.TOP, null, null));
		panel.setBounds(10, 11, 331, 252);
		getContentPane().add(panel);
		panel.setLayout(null);
		
		chkEther_DHCP = new JCheckBox("DHCP");
		chkEther_DHCP.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				chkEther_DHCP_actionPerformed();
			}
		});
		chkEther_DHCP.setBounds(193, 17, 115, 23);
		panel.add(chkEther_DHCP);
		
		lblSubnet = new JLabel("Subnet Mask:");
		lblSubnet.setHorizontalAlignment(SwingConstants.RIGHT);
		lblSubnet.setBounds(11, 75, 175, 14);
		panel.add(lblSubnet);
		
		lblGateway = new JLabel("Default Gateway:");
		lblGateway.setHorizontalAlignment(SwingConstants.RIGHT);
		lblGateway.setBounds(11, 106, 175, 14);
		panel.add(lblGateway);
		
		txtEther_Subnet = new JTextField();
		txtEther_Subnet.setBounds(196, 72, 112, 20);
		panel.add(txtEther_Subnet);
		txtEther_Subnet.setColumns(10);
		txtEther_Subnet.setText("255.255.255.0");
		
		txtEther_DefaultGateway = new JTextField();
		txtEther_DefaultGateway.setText("192.168.1.1");
		txtEther_DefaultGateway.setBounds(196, 103, 112, 20);
		panel.add(txtEther_DefaultGateway);
		txtEther_DefaultGateway.setColumns(10);
	
		btnGetEthernetSetting = new JButton("Get");
		btnGetEthernetSetting.setBounds(106, 218, 57, 23);
		panel.add(btnGetEthernetSetting);
		
		btnSetEthernetSetting = new JButton("Set");
		btnSetEthernetSetting.setBounds(195, 218, 59, 23);
		panel.add(btnSetEthernetSetting);
		
		lblIp = new JLabel("IP:");
		lblIp.setHorizontalAlignment(SwingConstants.RIGHT);
		lblIp.setBounds(11, 47, 175, 14);
		panel.add(lblIp);
		
		txtEther_IP = new JTextField();
		txtEther_IP.setText("192.168.1.224");
		txtEther_IP.setColumns(10);
		txtEther_IP.setBounds(196, 44, 112, 20);
		panel.add(txtEther_IP);
		
		label_1 = new JLabel("Primary DNS Server:");
		label_1.setHorizontalAlignment(SwingConstants.RIGHT);
		label_1.setBounds(11, 161, 175, 14);
		panel.add(label_1);
		
		txtEther_PrimaryDNSServer = new JTextField();
		txtEther_PrimaryDNSServer.setText("192.168.1.1");
		txtEther_PrimaryDNSServer.setColumns(10);
		txtEther_PrimaryDNSServer.setBounds(196, 158, 112, 20);
		panel.add(txtEther_PrimaryDNSServer);
		
		lblSecondaryDnsServer = new JLabel("Secondary DNS Server:");
		lblSecondaryDnsServer.setHorizontalAlignment(SwingConstants.RIGHT);
		lblSecondaryDnsServer.setBounds(11, 189, 175, 14);
		panel.add(lblSecondaryDnsServer);
		
		txtEther_SecondaryDNSServer = new JTextField();
		txtEther_SecondaryDNSServer.setColumns(10);
		txtEther_SecondaryDNSServer.setBounds(196, 186, 112, 20);
		panel.add(txtEther_SecondaryDNSServer);
		
		chkEther_ManualDNS = new JCheckBox("Manual DNS");
		chkEther_ManualDNS.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				chkEther_ManualDNS_actionPerformed();
			}
		});
		chkEther_ManualDNS.setSelected(true);
		chkEther_ManualDNS.setBounds(193, 131, 115, 23);
		panel.add(chkEther_ManualDNS);
		
		panel_1 = new JPanel();
		panel_1.setLayout(null);
		panel_1.setBorder(new TitledBorder(UIManager.getBorder("TitledBorder.border"), "WiFi Setting", TitledBorder.LEADING, TitledBorder.TOP, null, new Color(0, 0, 0)));
		panel_1.setBounds(351, 11, 331, 307);
		getContentPane().add(panel_1);
		
		chkWiFi_DHCP = new JCheckBox("DHCP");
		chkWiFi_DHCP.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				chkWiFi_DHCP_actionPerformed();
			}
		});
		chkWiFi_DHCP.setSelected(true);
		chkWiFi_DHCP.setBounds(196, 75, 112, 23);
		panel_1.add(chkWiFi_DHCP);
		
		lblSubnetMask = new JLabel("Subnet Mask:");
		lblSubnetMask.setHorizontalAlignment(SwingConstants.RIGHT);
		lblSubnetMask.setBounds(11, 133, 175, 14);
		panel_1.add(lblSubnetMask);
		
		lblDefaultGateway = new JLabel("Default Gateway:");
		lblDefaultGateway.setHorizontalAlignment(SwingConstants.RIGHT);
		lblDefaultGateway.setBounds(11, 164, 175, 14);
		panel_1.add(lblDefaultGateway);
		
		txtWiFi_Subnet = new JTextField();
		txtWiFi_Subnet.setColumns(10);
		txtWiFi_Subnet.setBounds(199, 130, 112, 20);
		panel_1.add(txtWiFi_Subnet);
		
		txtWiFi_DefaultGateway = new JTextField();
		txtWiFi_DefaultGateway.setColumns(10);
		txtWiFi_DefaultGateway.setBounds(199, 161, 112, 20);
		panel_1.add(txtWiFi_DefaultGateway);
		
		btnGetWiFiSetting = new JButton("Get");
		btnGetWiFiSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetWiFiSetting");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					txtWiFi_SSID.setText(SBXPCProxy.XML_ParseString(output.value, "SSID").value);
					txtWiFi_Key.setText(SBXPCProxy.XML_ParseString(output.value, "Key").value);
					chkWiFi_DHCP.setSelected(SBXPCProxy.XML_ParseInt(output.value, "DHCP") != 0);
					txtWiFi_IP.setText(SBXPCProxy.XML_ParseString(output.value, "IP").value);
					txtWiFi_Subnet.setText(SBXPCProxy.XML_ParseString(output.value, "Subnet").value);
					txtWiFi_DefaultGateway.setText(SBXPCProxy.XML_ParseString(output.value, "DefaultGateway").value);
					chkWiFi_ManualDNS.setSelected(SBXPCProxy.XML_ParseInt(output.value, "ManualDNS") != 0);
					txtWiFi_PrimaryDNSServer.setText(SBXPCProxy.XML_ParseString(output.value, "PrimaryDNSServer").value);
					txtWiFi_SecondaryDNSServer.setText(SBXPCProxy.XML_ParseString(output.value, "SecondaryDNSServer").value);
					
					chkWiFi_DHCP_actionPerformed();
					chkWiFi_ManualDNS_actionPerformed();
					JOptionPane.showMessageDialog(null, "Get WiFi Setting OK!");
				} else {
					JOptionPane.showMessageDialog(null, "Get WiFi Setting Failed");
				}
			}
		});
		btnGetWiFiSetting.setBounds(109, 273, 57, 23);
		panel_1.add(btnGetWiFiSetting);
		
		btnSetWiFiSetting = new JButton("Set");
		btnSetWiFiSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetWiFiSetting");

				output = SBXPCProxy.XML_AddString(output.value, "SSID", txtWiFi_SSID.getText());
				output = SBXPCProxy.XML_AddString(output.value, "Key", txtWiFi_Key.getText());
				output = SBXPCProxy.XML_AddInt(output.value, "DHCP", chkWiFi_DHCP.isSelected() ? 1 : 0);
				if (!chkWiFi_DHCP.isSelected())
				{
					output = SBXPCProxy.XML_AddString(output.value, "IP", txtWiFi_IP.getText());
					output = SBXPCProxy.XML_AddString(output.value, "Subnet", txtWiFi_Subnet.getText());
					output = SBXPCProxy.XML_AddString(output.value, "DefaultGateway", txtWiFi_DefaultGateway.getText());
				}
				output = SBXPCProxy.XML_AddInt(output.value, "ManualDNS", chkWiFi_ManualDNS.isSelected() ? 1 : 0);
				if (chkWiFi_ManualDNS.isSelected())
				{
					output = SBXPCProxy.XML_AddString(output.value, "PrimaryDNSServer", txtWiFi_PrimaryDNSServer.getText());
					output = SBXPCProxy.XML_AddString(output.value, "SecondaryDNSServer", txtWiFi_SecondaryDNSServer.getText());
				}

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					JOptionPane.showMessageDialog(null, "Set WiFi Setting OK!");
				} else {
					String str = SBXPCProxy.XML_ParseString(output.value, "Result").value;
					JOptionPane.showMessageDialog(null, "Set WiFi Setting Failed.\r\nResult:" + str);
				}	
			}
		});
		btnSetWiFiSetting.setBounds(198, 273, 59, 23);
		panel_1.add(btnSetWiFiSetting);
		
		lblIp_1 = new JLabel("IP:");
		lblIp_1.setHorizontalAlignment(SwingConstants.RIGHT);
		lblIp_1.setBounds(11, 105, 175, 14);
		panel_1.add(lblIp_1);
		
		txtWiFi_IP = new JTextField();
		txtWiFi_IP.setColumns(10);
		txtWiFi_IP.setBounds(199, 102, 112, 20);
		panel_1.add(txtWiFi_IP);
		
		label_6 = new JLabel("Primary DNS Server:");
		label_6.setHorizontalAlignment(SwingConstants.RIGHT);
		label_6.setBounds(11, 219, 175, 14);
		panel_1.add(label_6);
		
		txtWiFi_PrimaryDNSServer = new JTextField();
		txtWiFi_PrimaryDNSServer.setColumns(10);
		txtWiFi_PrimaryDNSServer.setBounds(199, 216, 112, 20);
		panel_1.add(txtWiFi_PrimaryDNSServer);
		
		lblSecondaryDnsServer_1 = new JLabel("Secondary DNS Server:");
		lblSecondaryDnsServer_1.setHorizontalAlignment(SwingConstants.RIGHT);
		lblSecondaryDnsServer_1.setBounds(11, 247, 175, 14);
		panel_1.add(lblSecondaryDnsServer_1);
		
		txtWiFi_SecondaryDNSServer = new JTextField();
		txtWiFi_SecondaryDNSServer.setColumns(10);
		txtWiFi_SecondaryDNSServer.setBounds(199, 244, 112, 20);
		panel_1.add(txtWiFi_SecondaryDNSServer);
		
		chkWiFi_ManualDNS = new JCheckBox("Manual DNS");
		chkWiFi_ManualDNS.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				chkWiFi_ManualDNS_actionPerformed();
			}
		});
		chkWiFi_ManualDNS.setBounds(196, 189, 112, 23);
		panel_1.add(chkWiFi_ManualDNS);
		
		txtWiFi_SSID = new JTextField();
		txtWiFi_SSID.setColumns(10);
		txtWiFi_SSID.setBounds(199, 21, 112, 20);
		panel_1.add(txtWiFi_SSID);
		
		lblSsid = new JLabel("SSID:");
		lblSsid.setHorizontalAlignment(SwingConstants.RIGHT);
		lblSsid.setBounds(11, 24, 175, 14);
		panel_1.add(lblSsid);
		
		lblKey = new JLabel("Key:");
		lblKey.setHorizontalAlignment(SwingConstants.RIGHT);
		lblKey.setBounds(11, 52, 175, 14);
		panel_1.add(lblKey);
		
		txtWiFi_Key = new JTextField();
		txtWiFi_Key.setColumns(10);
		txtWiFi_Key.setBounds(199, 49, 112, 20);
		panel_1.add(txtWiFi_Key);
		
		panel_2 = new JPanel();
		panel_2.setLayout(null);
		panel_2.setBorder(new TitledBorder(UIManager.getBorder("TitledBorder.border"), "Communication Setting", TitledBorder.LEADING, TitledBorder.TOP, null, new Color(0, 0, 0)));
		panel_2.setBounds(10, 274, 331, 206);
		getContentPane().add(panel_2);
		
		lblCommunicationPassword = new JLabel("Communication Password:");
		lblCommunicationPassword.setHorizontalAlignment(SwingConstants.RIGHT);
		lblCommunicationPassword.setBounds(10, 56, 175, 14);
		panel_2.add(lblCommunicationPassword);
		
		lblTcpPort = new JLabel("TCP Port:");
		lblTcpPort.setHorizontalAlignment(SwingConstants.RIGHT);
		lblTcpPort.setBounds(11, 87, 175, 14);
		panel_2.add(lblTcpPort);
		
		txtCommPwd = new JTextField();
		txtCommPwd.setText("0");
		txtCommPwd.setColumns(10);
		txtCommPwd.setBounds(196, 53, 112, 20);
		panel_2.add(txtCommPwd);
		
		txtTcpPort = new JTextField();
		txtTcpPort.setText("5005");
		txtTcpPort.setColumns(10);
		txtTcpPort.setBounds(196, 84, 112, 20);
		panel_2.add(txtTcpPort);
		
		btnGetCommSetting = new JButton("Get");
		btnGetCommSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetCommSetting");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					txtDeviceID.setText(SBXPCProxy.XML_ParseString(output.value, "DeviceID").value);
					txtCommPwd.setText(SBXPCProxy.XML_ParseString(output.value, "CommPwd").value);
					txtTcpPort.setText(SBXPCProxy.XML_ParseString(output.value, "TcpPort").value);
					txtP2P_Server.setText(SBXPCProxy.XML_ParseString(output.value, "P2PSvr").value);
					txtP2P_Port.setText(SBXPCProxy.XML_ParseString(output.value, "P2PPort").value);
					JOptionPane.showMessageDialog(null, "Get Communication Setting OK!");
				} else {
					JOptionPane.showMessageDialog(null, "Get Communication Setting Failed");
				}	
			}
		});
		btnGetCommSetting.setBounds(106, 168, 57, 23);
		panel_2.add(btnGetCommSetting);
		
		btnSetCommSetting = new JButton("Set");
		btnSetCommSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetCommSetting");

				output = SBXPCProxy.XML_AddLong(output.value, "DeviceID", Integer.parseInt(txtDeviceID.getText()));
				output = SBXPCProxy.XML_AddLong(output.value, "CommPwd", Integer.parseInt(txtCommPwd.getText()));
				output = SBXPCProxy.XML_AddLong(output.value, "TcpPort", Integer.parseInt(txtTcpPort.getText()));
				output = SBXPCProxy.XML_AddString(output.value, "P2PSvr", txtP2P_Server.getText());
				output = SBXPCProxy.XML_AddLong(output.value, "P2PPort", Integer.parseInt(txtP2P_Port.getText()));

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					JOptionPane.showMessageDialog(null, "Set Communication Setting OK!");
				} else {
					String str = SBXPCProxy.XML_ParseString(output.value, "Result").value;
					JOptionPane.showMessageDialog(null, "Set Communication Setting Failed.\r\nResult:" + str);
				}	
			}
		});
		btnSetCommSetting.setBounds(195, 168, 59, 23);
		panel_2.add(btnSetCommSetting);
		
		lblDeviceid = new JLabel("DeviceID:");
		lblDeviceid.setHorizontalAlignment(SwingConstants.RIGHT);
		lblDeviceid.setBounds(11, 28, 175, 14);
		panel_2.add(lblDeviceid);
		
		txtDeviceID = new JTextField();
		txtDeviceID.setText("1");
		txtDeviceID.setColumns(10);
		txtDeviceID.setBounds(196, 25, 112, 20);
		panel_2.add(txtDeviceID);
		
		lblPpServerIp = new JLabel("P2P Server IP:");
		lblPpServerIp.setHorizontalAlignment(SwingConstants.RIGHT);
		lblPpServerIp.setBounds(11, 115, 175, 14);
		panel_2.add(lblPpServerIp);
		
		txtP2P_Server = new JTextField();
		txtP2P_Server.setColumns(10);
		txtP2P_Server.setBounds(196, 112, 112, 20);
		panel_2.add(txtP2P_Server);
		
		lblPpServerPort = new JLabel("P2P Server Port:");
		lblPpServerPort.setHorizontalAlignment(SwingConstants.RIGHT);
		lblPpServerPort.setBounds(11, 143, 175, 14);
		panel_2.add(lblPpServerPort);
		
		txtP2P_Port = new JTextField();
		txtP2P_Port.setColumns(10);
		txtP2P_Port.setBounds(196, 140, 112, 20);
		panel_2.add(txtP2P_Port);
		
		btnApplyCommSetting = new JButton("ApplyCommSetting");
		btnApplyCommSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("ApplyCommSetting");

				output = SBXPCProxy.XML_AddLong(output.value, "Apply", 1);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					JOptionPane.showMessageDialog(null, "Apply Settings OK!");
				} else {
					String str = SBXPCProxy.XML_ParseString(output.value, "Result").value;
					JOptionPane.showMessageDialog(null, "Apply Settings Failed.\r\nResult:" + str);
				}
			}
		});
		btnApplyCommSetting.setBounds(454, 416, 146, 23);
		getContentPane().add(btnApplyCommSetting);
		btnSetEthernetSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetEthernetSetting");

				output = SBXPCProxy.XML_AddInt(output.value, "DHCP", chkEther_DHCP.isSelected() ? 1 : 0);
				if (!chkEther_DHCP.isSelected())
				{
					output = SBXPCProxy.XML_AddString(output.value, "IP", txtEther_IP.getText());
					output = SBXPCProxy.XML_AddString(output.value, "Subnet", txtEther_Subnet.getText());
					output = SBXPCProxy.XML_AddString(output.value, "DefaultGateway", txtEther_DefaultGateway.getText());
				}
				output = SBXPCProxy.XML_AddInt(output.value, "ManualDNS", chkEther_ManualDNS.isSelected() ? 1 : 0);
				if (chkEther_ManualDNS.isSelected())
				{
					output = SBXPCProxy.XML_AddString(output.value, "PrimaryDNSServer", txtEther_PrimaryDNSServer.getText());
					output = SBXPCProxy.XML_AddString(output.value, "SecondaryDNSServer", txtEther_SecondaryDNSServer.getText());
				}

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					JOptionPane.showMessageDialog(null, "Set Ethernet Setting OK!");
				} else {
					String str = SBXPCProxy.XML_ParseString(output.value, "Result").value;
					JOptionPane.showMessageDialog(null, "Set Ethernet Setting Failed.\r\nResult:" + str);
				}	
			}
		});
		btnGetEthernetSetting.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetEthernetSetting");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					chkEther_DHCP.setSelected(SBXPCProxy.XML_ParseInt(output.value, "DHCP") != 0);
					txtEther_IP.setText(SBXPCProxy.XML_ParseString(output.value, "IP").value);
					txtEther_Subnet.setText(SBXPCProxy.XML_ParseString(output.value, "Subnet").value);
					txtEther_DefaultGateway.setText(SBXPCProxy.XML_ParseString(output.value, "DefaultGateway").value);
					chkEther_ManualDNS.setSelected(SBXPCProxy.XML_ParseInt(output.value, "ManualDNS") != 0);
					txtEther_PrimaryDNSServer.setText(SBXPCProxy.XML_ParseString(output.value, "PrimaryDNSServer").value);
					txtEther_SecondaryDNSServer.setText(SBXPCProxy.XML_ParseString(output.value, "SecondaryDNSServer").value);
					
					chkEther_DHCP_actionPerformed();
					chkEther_ManualDNS_actionPerformed();
					JOptionPane.showMessageDialog(null, "Get Ethernet Setting OK!");
				} else {
					JOptionPane.showMessageDialog(null, "Get Ethernet Setting Failed");
				}	
			}
		});
	}
	private void chkEther_DHCP_actionPerformed() {
        txtEther_IP.setEnabled(!chkEther_DHCP.isSelected());
        txtEther_Subnet.setEnabled(!chkEther_DHCP.isSelected());
        txtEther_DefaultGateway.setEnabled(!chkEther_DHCP.isSelected());
	}
	private void chkEther_ManualDNS_actionPerformed() {
        txtEther_PrimaryDNSServer.setEnabled(chkEther_ManualDNS.isSelected());
        txtEther_SecondaryDNSServer.setEnabled(chkEther_ManualDNS.isSelected());
	}
	private void chkWiFi_DHCP_actionPerformed() {
        txtWiFi_IP.setEnabled(!chkWiFi_DHCP.isSelected());
        txtWiFi_Subnet.setEnabled(!chkWiFi_DHCP.isSelected());
        txtWiFi_DefaultGateway.setEnabled(!chkWiFi_DHCP.isSelected());
	}
	private void chkWiFi_ManualDNS_actionPerformed() {
        txtWiFi_PrimaryDNSServer.setEnabled(chkWiFi_ManualDNS.isSelected());
        txtWiFi_SecondaryDNSServer.setEnabled(chkWiFi_ManualDNS.isSelected());
	}
}
