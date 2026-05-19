function utf16Encode(str) {
	var result = "", offset = 0, length, c;
	
	length = str.length;
	while (offset < length) {
		c = str[offset].charCodeAt(0);
		offset += 1;
		
		var ch = c >> 8;
		var cl = c & 0xff;
		result = result + String.fromCharCode(cl);
		result = result + String.fromCharCode(ch);
	}
	result = result + String.fromCharCode(0);
	result = result + String.fromCharCode(0);

	return result;
}

function utf16Decode(bytes) {
	var chars = [], offset = 0, length = bytes.length;
	while (offset < length) {
		var cl = bytes[offset].charCodeAt(0);
		var ch = bytes[offset + 1].charCodeAt(0);

		if (ch != 0 || cl != 0)
			chars.push(String.fromCharCode(ch << 8 | cl));
		offset += 2;
	}
	
	return chars.join('');
}