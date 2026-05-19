var ws = null;
		
function ws_init(port, use_wss) {
	// Connect to Web Socket
	if (ws != null)
	{
		ws.close();
		ws = null;
	}
			
	var uri = window.location.href;
	var pos = uri.indexOf("://") + 3;	// remove prefix
	uri = uri.substring(pos);
			
	pos = uri.indexOf("/");	// remove postfix
	if (pos > 0)
		uri = uri.substring(0, pos);
			
	pos = uri.indexOf(":");
	if (pos > 0)
		uri = uri.substring(0, pos);

    if (use_wss)
        uri = "wss://" + uri + ":" + port;
    else
        uri = "ws://" + uri + ":" + port;

    ws = new WebSocket(uri);
}
		
function ws_exit() {
	if (ws != null)
	{
		ws.close();
		ws = null;
	}
}

function send_relay_message(doc, session, message) {

	if (ws == null)
	{
		alert("Can't connect to server");
		return;
	}

	var rootElem = doc.createElement("RelayMessage");

	var sessionElem = doc.createElement("Session");
	sessionElem.innerHTML = session;
	rootElem.appendChild(sessionElem);
	rootElem.appendChild(message);

	doc.appendChild(rootElem);

	
	serializer = new XMLSerializer();
	ws.send(serializer.serializeToString(doc));
}