# OpenWindesheart-Demo

This project features a mobile demo-application for the OpenWindesheart library.
For detailed docs visit [OpenWindesheart](https://github.com/ictinnovaties-zorg/openwindesheart)

## Support

Currently this project only fully supports the Mi Band 3.

If you want these features with a Mi Band 4, then you will have to get the authentication-key for your band here:
https://www.freemyband.com/2019/08/mi-band-4-auth-key.html

We have a branch for the Mi Band 4, called MiBand4, which allows these devices to show up in the list of scanned devices.

To make your app work with our logic, please update line 13 of this file: https://github.com/ictinnovaties-zorg/openwindesheart/blob/MiBand4/WindesHeartSDK/Devices/MiBand4/Helpers/MiBand4ConversionHelper.cs 

This is where you fill in your own secret key. For example; the key '0a1bc2' should be altered to {0x0a, 0x1b, 0xc2} in the byte-array on line 13 of the code.

After that, you will only have to build the SDK and the mobile-project. 
Once all is built, it should be ready for use with the Mi Band 4.

## Creators

* R. Abächerli [@ramonb1996](https://github.com/ramonB1996)
* H. van der Gugten [@hielkeg](https://github.com/hielkeg)
* T.C. Marschalk [@marstc](https://github.com/marstc)
* K. van Sloten [@kevinvansloten](https://github.com/kevinvansloten)

## Copyright

Copyright 2020 Research group ICT innovations in Health Care, Windesheim University of Applied Sciences.
