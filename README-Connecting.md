# Connecting
This documentation explains and demonstrates how to connect with a Bluetooth device. You should have access to the device itself by scanning for it. If you haven't, please take a look at the scanning docs.

**Please note:** The SDK only supports one device to be connected at a time.

## BLEDevice.Connect(callback, secretkey)
This method attempts to connect with your device. The callback parameter will be called when the connection process has finished. **This doesn't mean that it's always successful!**
The callback returns a `ConnectionResult` enum. This enum can be either `Succeeded`, or `Failed`.

You can also give this method the secretkey of the device you're trying to connect with. If you don't give a key to the connect method, It will generate a new key. This key will be returned in the connection callback. It is important you save this key. The next time you want to connect to this device you'll need use this key with the connect method. This is needed to reconnect without losing data! 

Example:
```
BLEDevice device = //Retrieved from scanning
device.Connect(OnConnectionFinished);
```
```csharp
//Our scanning callback method
void OnDeviceFound(BLEScanResult result){
   BLEDevice device = result.Device;
   //let's first stop scanning
   Windesheart.StopScanning();
   //Then, connect to device
   device.Connect(OnConnectionFinished);
}

void OnConnectionFinished(ConnectionResult result, byte[] secretKey){
   if (result == ConnectionResult.Succeeded)
   {
       Console.WriteLine("Successful Connection!");
       SaveKeyToProperties(secretKey)
   }
   else
   {
       Console.WriteLine("Connection failed... :(");
   }
}
```

## BLEDevice.Disconnect(rememberDevice = true)
This method disconnects the BLEDevice. There is an optional parameter called rememberDevice that defaults to true.

WindesHeart always saves your currently connected device to the `Windesheart.ConnectedDevice` field. if rememberDevice is true, it won't clear this field after disconnecting. If false is passed, `Windesheart.ConnectedDevice` will be set to null.

The disconnect method is a void. You can assume that it always works.

[<---- Back to mainpage](https://github.com/ictinnovaties-zorg/openwindesheart-demo)