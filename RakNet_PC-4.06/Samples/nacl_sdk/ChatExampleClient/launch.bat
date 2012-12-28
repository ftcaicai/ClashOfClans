SET NACL_SRPC_DEBUG=2
SET NACL_PLUGIN_DEBUG=2
SET NACL_DEBUG_ENABLE=1
SET NACL_PPAPI_PROXY_DEBUG=2
SET PPAPI_BROWSER_DEBUG=1
SET NACLVERBOSITY=4

REM "C:\Users\Kevin\AppData\Local\Google\Chrome\Application\chrome.exe" --allow-nacl-socket-api=127.0.0.1 --enable-nacl-debug --no-sandbox --load-extension="D:\RakNet\Samples\nacl_sdk\ChatExampleClient" --enable-nacl --disk-cache-size=1 --chrome-version21.0.1180.89

"C:\Users\Kevin\AppData\Local\Google\Chrome\Application\chrome.exe" --allow-nacl-socket-api=127.0.0.1 --enable-nacl-debug --no-sandbox --load-extension="D:\RakNet\Samples\nacl_sdk\ChatExampleClient" --enable-nacl --disk-cache-size=1 --chrome-version22.0.1229.56