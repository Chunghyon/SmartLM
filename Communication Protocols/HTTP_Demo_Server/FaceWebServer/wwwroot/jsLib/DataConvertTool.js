function DataConvertTool() {

    var obj = {
        /**
         * base64转buffer对象
         * @param {any} imgElement
         * @returns  Buffer
         */
        Base64ToBuffer: function (strBase64) {
            if (strBase64.length == 0) {
                return undefined;
            }

            var filedata = window.atob(strBase64);
            var buffer = new Uint8Array(filedata.length);
            for (var i = 0; i < filedata.length; i++) {
                buffer[i] = filedata.charCodeAt(i);
            }
            return buffer;
        },
        //计算特征码MD5
        //需要先导入动态库 <script src="/jsLib/crypto-js.min.js"></script>
        CreateMD5Hex_Uint8Array: function (bBuf) {
            // 将 Uint8Array 转换为 WordArray
            const wordArray = CryptoJS.lib.WordArray.create(bBuf);
            return CryptoJS.MD5(wordArray).toString(
                CryptoJS.enc.Hex
            ).toUpperCase();
        },
        //创建Base64字符串的MD5，返回MD5的十六进制编码
        CreateBase64StrMD5Hex: function (sBase64) {
            
            var bBuf = this.Base64ToBuffer(sBase64);
            return this.CreateMD5Hex_Uint8Array(bBuf);

        }


    }

    return obj;
};
