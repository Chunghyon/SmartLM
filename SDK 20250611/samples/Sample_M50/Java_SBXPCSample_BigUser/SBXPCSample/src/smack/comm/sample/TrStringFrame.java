package smack.comm.sample;

import java.awt.EventQueue;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.Base64;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JTextField;
import javax.swing.SwingConstants;

import smack.comm.SBXPCProxy;
import smack.comm.output.OneStringOutput;
import smack.comm.sample.global.SysUtil;

public class TrStringFrame extends JFrame {
	private JLabel lblMessage;
	private JLabel lblTrNo;
	private JLabel lblTrString;
	private JTextField txtTrNo;
	private JTextField txtTrString;
	private JButton btnRead;
	private JButton btnWrite;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					TrStringFrame frame = new TrStringFrame();
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
	public TrStringFrame() {
		addWindowListener(new WindowAdapter() {
			@Override
			public void windowClosing(WindowEvent arg0) {
				if (MainFrame.getInstance() != null)
					MainFrame.getInstance().setVisible(true);
			}
		});
	
		setTitle("Tr String");
		setBounds(100, 100, 388, 295);
		setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
		getContentPane().setLayout(null);
		
		lblMessage = new JLabel("Message");
		lblMessage.setHorizontalAlignment(SwingConstants.CENTER);
		lblMessage.setFont(new Font("Segoe UI", Font.BOLD, 18));
		lblMessage.setBorder(javax.swing.BorderFactory.createBevelBorder(javax.swing.border.BevelBorder.LOWERED));
		lblMessage.setBounds(10, 11, 352, 40);
		getContentPane().add(lblMessage);
		
		lblTrNo = new JLabel("TrNo: ");
		lblTrNo.setBounds(10, 64, 115, 14);
		getContentPane().add(lblTrNo);
		
		lblTrString = new JLabel("TrString: ");
		lblTrString.setBounds(10, 95, 115, 14);
		getContentPane().add(lblTrString);
		
		txtTrNo = new JTextField();
		txtTrNo.setBounds(135, 64, 227, 20);
		getContentPane().add(txtTrNo);
		txtTrNo.setColumns(10);
		txtTrNo.setText("0");
		
		txtTrString = new JTextField();
		txtTrString.setBounds(135, 95, 227, 20);
		getContentPane().add(txtTrString);
		txtTrString.setColumns(10);
		txtTrString.setText("Duty On");
	
		btnRead = new JButton("Read");
		btnRead.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				int TrNo = -1;
				try {
					TrNo = Integer.parseInt(txtTrNo.getText());
				} catch (NumberFormatException ne) {
				}

				if (TrNo < 0 || TrNo >= 6)
				{
					JOptionPane.showMessageDialog(null, "TrNo Invalid!");
					txtTrNo.requestFocus();
					txtTrNo.selectAll();
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("GetTrString");
				output = SBXPCProxy.XML_AddLong(output.value, "TrNo", TrNo);

				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
	                lblMessage.setText("Tr[" + txtTrNo.getText() + "]: ");
	                
					String base64_name = SBXPCProxy.XML_ParseString(output.value, "TrName").value;
					if (base64_name != null)
					{
						try 
						{
							byte[] name_binary = Base64.getDecoder().decode(base64_name);
							int index = 0;
                            for (int i = 0; i < name_binary.length - 1; i += 2)
                            {
                                if (name_binary[i] == 0 && name_binary[i + 1] == 0)
                                {
                                    index = i;
                                    break;
                                }
                            }
							char[] char_binary = new char[index / 2];
							for (int i = 0; i < index / 2; i++)
							{
								int hi = name_binary[i * 2 + 1];
								int lo = name_binary[i * 2];
								if (hi < 0) hi += 256;
								if (lo < 0) lo += 256;
								
								char_binary[i] = (char)(lo + hi * 256);
							}
							
                            txtTrString.setText(String.valueOf(char_binary, 0, index / 2));
                            lblMessage.setText(lblMessage.getText() + txtTrString.getText());
						}
						catch(Exception ex)
						{
							lblMessage.setText("Error while Base64 decoding.");
						}
					}
					
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnRead.setBounds(10, 185, 171, 28);
		getContentPane().add(btnRead);
		
		btnWrite = new JButton("Write");
		btnWrite.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {
				int TrNo = -1;
				try {
					TrNo = Integer.parseInt(txtTrNo.getText());
				} catch (NumberFormatException ne) {
				}

				if (TrNo < 0 || TrNo >= 6)
				{
					JOptionPane.showMessageDialog(null, "TrNo Invalid!");
					txtTrNo.requestFocus();
					txtTrNo.selectAll();
					return;
				}

				lblMessage.setText(SysUtil.WORKING);
				invalidate();

				int errorCode;
				OneStringOutput output;
				output = SysUtil.MakeXMLCommandHeader("SetTrString");
				output = SBXPCProxy.XML_AddLong(output.value, "TrNo", TrNo);
				{
					char[] chs = txtTrString.getText().toCharArray();
					byte[] bys = new byte[chs.length * 2];
					for (int i = 0; i < chs.length; i++)
					{
						bys[i * 2 + 1] = (byte)(chs[i] / 256);
						bys[i * 2] = (byte)(chs[i] & 0xFF);
					}
					output = SBXPCProxy.XML_AddString(output.value, "TrName", Base64.getEncoder().encodeToString(bys));
				}
				output = SBXPCProxy.GeneralOperationXML(SysUtil.MachineNumber, output.value);

				if (output.isSuccess()) {
					String error_info = SBXPCProxy.XML_ParseString(output.value, "Error").value;
					if (error_info.equals("Success"))
						lblMessage.setText("SetOK. Tr[" + txtTrNo.getText() + "]: " + txtTrString.getText());
					else
						lblMessage.setText("Error: " + error_info);
				
				} else {
					errorCode = (int) SBXPCProxy.GetLastError(SysUtil.MachineNumber).dwValue;
					lblMessage.setText(SysUtil.ErrorPrint(errorCode));
				}
			}
		});
		btnWrite.setBounds(191, 185, 171, 28);
		getContentPane().add(btnWrite);
	}
}
