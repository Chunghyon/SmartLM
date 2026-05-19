package access;

/**
 *
 */
public class Device {
    public Device(String deviceSn, String password, int clientID) {
        this.deviceSn = deviceSn;
        this.password = password;
        this.clientID = clientID;
    }

    /**
     * Device SN
     */
    private String deviceSn;
    /**
     * Device communication password
     */
    private String password;
    /**
     * Unique identifier of the client connecting to the server
     */
    private int clientID;

    /**
     * Obtain the unique ID of the device client
     *
     * @return
     */
    public int getClientID() {
        return clientID;
    }

    /**
     * Set a unique client ID
     * @param clientID
     */
    public void setClientID(int clientID) {
        this.clientID = clientID;
    }

    /**
     * Get device SN
     *
     * @return
     */
    public String getDeviceSn() {
        return deviceSn;
    }

    /**
     * Obtain device communication password
     *
     * @return
     */
    public String getPassword() {
        return password;
    }
}
