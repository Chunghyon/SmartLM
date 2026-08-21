namespace FaceDeviceHttpPcServer.Forms;

public sealed class FdhsTrayContext : ApplicationContext
{
    private readonly MainForm _form;

    public FdhsTrayContext(MainForm form)
    {
        _form = form;
        _form.FormClosed += (_, _) => ExitThread();
    }
}
