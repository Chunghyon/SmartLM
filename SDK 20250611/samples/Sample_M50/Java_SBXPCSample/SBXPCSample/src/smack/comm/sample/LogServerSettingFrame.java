package smack.comm.sample;

import java.awt.EventQueue;
import java.awt.Font;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.IOException;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.SwingConstants;

import smack.comm.SBXPCProxy;
import smack.comm.output.OneStringOutput;
import smack.comm.sample.global.SysUtil;
import sun.misc.BASE64Decoder;

import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;

public class LogServerSettingFrame extends JFrame {
	private JLabel lblMessage;
	private JLabel lblAddr;
	private JLabel lblPort;
	private JTextField txtAddr;
	private JTextField txtPort;
	private JButton btnRead;
	private JButton btnWrite;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					LogServerSettingFrame frame = new LogServerSettingFrame();
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
	public LogServerSettingFrame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});
	
		setTitle("Log Server Setting");
		setBounds(100, 100, 388, 295);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);
		
		lblMessage = new JLabel("Message");
		lblMessage.setHorizontalAlignment(SwingConstants.CENTER);
		lblMessage.setFont(new Font("Segoe UI", Font.BOLD, 18));
		lblMessage.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
		lblMessage.setBounds(10, 11, 352, 40);
		getContentPane().add(lblMessage);
		
		lblAddr = new JLabel("Server Domain name or IP: ");
		lblAddr.setBounds(10, 64, 160, 14);
		getContentPane().add(lblAddr);
		
		lblPort = new JLabel("Port: ");
		lblPort.setBounds(10, 126, 115, 14);
		getContentPane().add(lblPort);
		
		txtAddr = new JTextField();
		txtAddr.setBounds(135, 95, 227, 20);
		getContentPane().add(txtAddr);
		txtAddr.setColumns(10);
		txtAddr.setText("logserver.test.domain");
		
		txtPort = new JTextField();
		txtPort.setBounds(135, 157, 227, 20);
		getContentPane().add(txtPort);
		txtPort.setColumns(10);
		txtPort.setText("5005");
		
		btnRead = new JButton("Get");
		btnRead.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetLogServerSetting");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					String ManagerPCDomainName = SBXPCProxy.XML_ParseString(output.value, "ManagerPCDomainName").value;
					int ManagerPCPort = SBXPCProxy.XML_ParseInt(output.value, "ManagerPCPort");
					txtAddr.setText(ManagerPCDomainName);
					txtPort.setText(String.valueOf(ManagerPCPort));
					lblMessage.setText("Get Settings OK!");
				} else {
					lblMessage.setText("Get Settings Failed");
				}
			}
		});
		btnRead.setBounds(10, 200, 171, 28);
		getContentPane().add(btnRead);
		
		btnWrite = new JButton("Set");
		btnWrite.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetLogServerSetting");
				output = SBXPCProxy.XML_AddString(output.value, "ManagerPCDomainName", txtAddr.getText());
				output = SBXPCProxy.XML_AddLong(output.value, "ManagerPCPort", Integer.parseInt(txtPort.getText()));
				
				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					lblMessage.setText("Set Settings OK!");
				} else {
					String Result = SBXPCProxy.XML_ParseString(output.value, "Result").value;
					lblMessage.setText("Set Settings Failed.\n Result: " + Result);
				}
			}
		});
		btnWrite.setBounds(191, 200, 171, 28);
		getContentPane().add(btnWrite);
	}
}
