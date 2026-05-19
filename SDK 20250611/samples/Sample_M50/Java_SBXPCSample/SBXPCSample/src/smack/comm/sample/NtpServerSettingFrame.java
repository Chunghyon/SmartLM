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

public class NtpServerSettingFrame extends JFrame {
	private JLabel lbl1;
	private JLabel lbl2;
	private JTextField txtServerAddress;
	private JTextField txtTimezone;
	private JButton btnGet;
	private JButton btnSet;
	private JLabel lbl3;
	private JTextField txtInterval;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					NtpServerSettingFrame frame = new NtpServerSettingFrame();
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
	public NtpServerSettingFrame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});
	
		setTitle("NtpServerSetting");
		setBounds(100, 100, 388, 295);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);
		
		lbl1 = new JLabel("NTP Server Address: ");
		lbl1.setHorizontalAlignment(SwingConstants.RIGHT);
		lbl1.setBounds(10, 48, 157, 14);
		getContentPane().add(lbl1);
		
		lbl2 = new JLabel("Timezone: ");
		lbl2.setHorizontalAlignment(SwingConstants.RIGHT);
		lbl2.setBounds(10, 88, 157, 14);
		getContentPane().add(lbl2);
		
		txtServerAddress = new JTextField();
		txtServerAddress.setBounds(177, 48, 185, 20);
		getContentPane().add(txtServerAddress);
		txtServerAddress.setColumns(10);
		txtServerAddress.setText("1.cn.pool.ntp.org");
		
		txtTimezone = new JTextField();
		txtTimezone.setBounds(177, 88, 185, 20);
		getContentPane().add(txtTimezone);
		txtTimezone.setColumns(10);
		txtTimezone.setText("480");
	
		btnGet = new JButton("Get");
		btnGet.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetDeviceInfoExt");
				output = SBXPCProxy.XML_AddString(output.value, "ParamName", "NTPServer");

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					txtServerAddress.setText(SBXPCProxy.XML_ParseString(output.value, "Value1").value);
					txtTimezone.setText(SBXPCProxy.XML_ParseString(output.value, "Value2").value);
					txtInterval.setText(SBXPCProxy.XML_ParseString(output.value, "Value3").value);

					JOptionPane.showMessageDialog(null, "Get NTP Server Settings OK!");
				} else {
					JOptionPane.showMessageDialog(null, "Get NTP Server Settings Failed.");
				}	
			}
		});
		btnGet.setBounds(82, 185, 99, 28);
		getContentPane().add(btnGet);
		
		btnSet = new JButton("Set");
		btnSet.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetDeviceInfoExt");
				output = SBXPCProxy.XML_AddString(output.value, "ParamName", "NTPServer");

				output = SBXPCProxy.XML_AddString(output.value, "Value1", txtServerAddress.getText());
				output = SBXPCProxy.XML_AddLong(output.value, "Value2", Integer.parseInt(txtTimezone.getText()));
				output = SBXPCProxy.XML_AddLong(output.value, "Value3", Integer.parseInt(txtInterval.getText()));

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);
				if (output.isSuccess()) {
					JOptionPane.showMessageDialog(null, "Set NTP Server Settings OK!");
				} else {
					String str = SBXPCProxy.XML_ParseString(output.value, "Result").value;
					JOptionPane.showMessageDialog(null, "Set NTP Server Settings Failed.\r\nResult:" + str);
				}	
			}
		});
		btnSet.setBounds(191, 185, 99, 28);
		getContentPane().add(btnSet);
		
		lbl3 = new JLabel("Interval: ");
		lbl3.setHorizontalAlignment(SwingConstants.RIGHT);
		lbl3.setBounds(10, 130, 157, 14);
		getContentPane().add(lbl3);
		
		txtInterval = new JTextField();
		txtInterval.setText("60");
		txtInterval.setColumns(10);
		txtInterval.setBounds(177, 130, 185, 20);
		getContentPane().add(txtInterval);
	}
}
