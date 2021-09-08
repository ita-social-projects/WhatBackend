set connection-string=""DefaultEndpointsProtocol=https;AccountName=whatbackendstorage;AccountKey=8lsbrQH1kCvf2FGpbuojyvzDA/nHhOiGHvkgzRGriSSBRToIum8HiyudEAlTk7AVmjLQGuNObriGXMQIIxVjIQ==;EndpointSuffix=core.windows.net""
call az storage container create -n 9eafe44946544d49ab04c6c1de3d191c --connection-string %connection-string%
call az storage blob upload -f homework-from-students\1.txt -c 9eafe44946544d49ab04c6c1de3d191c -n 1.txt --connection-string %connection-string%
call az storage container create -n ce316397fc92414098f548fc998647fa --connection-string %connection-string%
call az storage blob upload -f homework-from-students\2.txt -c ce316397fc92414098f548fc998647fa -n 2.txt --connection-string %connection-string%
call az storage container create -n f91021be6d864ff48b18620ab7796d86 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\3.txt -c f91021be6d864ff48b18620ab7796d86 -n 3.txt --connection-string %connection-string%
call az storage container create -n 7dd33c736c8a44fba052c033b6cd5742 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\4.txt -c 7dd33c736c8a44fba052c033b6cd5742 -n 4.txt --connection-string %connection-string%
call az storage container create -n 35c1beb1b02e4bc7a9425aacb7bec973 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\5.txt -c 35c1beb1b02e4bc7a9425aacb7bec973 -n 5.txt --connection-string %connection-string%
call az storage container create -n 415ad6a6156944f19d7243fb1c9d72d5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\6.txt -c 415ad6a6156944f19d7243fb1c9d72d5 -n 6.txt --connection-string %connection-string%
call az storage container create -n 396410823c8d465fad37eab4d4c26e76 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\7.txt -c 396410823c8d465fad37eab4d4c26e76 -n 7.txt --connection-string %connection-string%
call az storage container create -n 06c6673a66f144a5a9109831de1969de --connection-string %connection-string%
call az storage blob upload -f homework-from-students\8.txt -c 06c6673a66f144a5a9109831de1969de -n 8.txt --connection-string %connection-string%
call az storage container create -n 8764abb376d0458da57985f981405c56 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\9.txt -c 8764abb376d0458da57985f981405c56 -n 9.txt --connection-string %connection-string%
call az storage container create -n 456206da556844dfb96527aa7528e871 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\10.txt -c 456206da556844dfb96527aa7528e871 -n 10.txt --connection-string %connection-string%
call az storage container create -n 3c24a61cdc4f4f7fb01088563e0059ab --connection-string %connection-string%
call az storage blob upload -f homework-from-students\11.txt -c 3c24a61cdc4f4f7fb01088563e0059ab -n 11.txt --connection-string %connection-string%
call az storage container create -n 88472ce1fec04276af4cf0b4da9e7bc5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\12.txt -c 88472ce1fec04276af4cf0b4da9e7bc5 -n 12.txt --connection-string %connection-string%
call az storage container create -n 953ce34a3cb44313b34c502f8e547f7a --connection-string %connection-string%
call az storage blob upload -f homework-from-students\13.txt -c 953ce34a3cb44313b34c502f8e547f7a -n 13.txt --connection-string %connection-string%
call az storage container create -n d38dd009d3324b7f9bf23f0f9b5b3f88 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\14.txt -c d38dd009d3324b7f9bf23f0f9b5b3f88 -n 14.txt --connection-string %connection-string%
call az storage container create -n b7bdcf30861d4bfd9976554735ce3ba5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\15.txt -c b7bdcf30861d4bfd9976554735ce3ba5 -n 15.txt --connection-string %connection-string%
call az storage container create -n 6f87c132d2eb4a06b1ce6169321f314b --connection-string %connection-string%
call az storage blob upload -f homework-from-students\16.txt -c 6f87c132d2eb4a06b1ce6169321f314b -n 16.txt --connection-string %connection-string%
call az storage container create -n f850570e2af74d308833fae1169a72b9 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\17.txt -c f850570e2af74d308833fae1169a72b9 -n 17.txt --connection-string %connection-string%
call az storage container create -n 26e3d732a8dc4790adef600053cd375e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\18.txt -c 26e3d732a8dc4790adef600053cd375e -n 18.txt --connection-string %connection-string%
call az storage container create -n fac6e83ebb4047229238275b56236817 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\19.txt -c fac6e83ebb4047229238275b56236817 -n 19.txt --connection-string %connection-string%
call az storage container create -n 227c184b57e4428e9c0617f6d1ae98a5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\20.txt -c 227c184b57e4428e9c0617f6d1ae98a5 -n 20.txt --connection-string %connection-string%
call az storage container create -n ae7ce45aaa1a446db50eb4df1741395c --connection-string %connection-string%
call az storage blob upload -f homework-from-students\21.txt -c ae7ce45aaa1a446db50eb4df1741395c -n 21.txt --connection-string %connection-string%
call az storage container create -n 00506e20d402497faf118cbb70eb38ba --connection-string %connection-string%
call az storage blob upload -f homework-from-students\22.txt -c 00506e20d402497faf118cbb70eb38ba -n 22.txt --connection-string %connection-string%
call az storage container create -n f50734aa7a4a4858b5a40d7e3dc1c25e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\23.txt -c f50734aa7a4a4858b5a40d7e3dc1c25e -n 23.txt --connection-string %connection-string%
call az storage container create -n 975977663e074587a28da52fd664a0c2 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\24.txt -c 975977663e074587a28da52fd664a0c2 -n 24.txt --connection-string %connection-string%
call az storage container create -n d1e78873161040bc97e7e82868a4ddb0 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\25.txt -c d1e78873161040bc97e7e82868a4ddb0 -n 25.txt --connection-string %connection-string%
call az storage container create -n 9f51b509a95e411ba55f79fd430653e2 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\26.txt -c 9f51b509a95e411ba55f79fd430653e2 -n 26.txt --connection-string %connection-string%
call az storage container create -n 51187423f2fc429ab97f68cd1c8a4739 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\27.txt -c 51187423f2fc429ab97f68cd1c8a4739 -n 27.txt --connection-string %connection-string%
call az storage container create -n d9843bbef6514dc2a786e8373806d928 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\28.txt -c d9843bbef6514dc2a786e8373806d928 -n 28.txt --connection-string %connection-string%
call az storage container create -n 6bfd540f63ec4e629ccfba3ce1322ef9 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\29.txt -c 6bfd540f63ec4e629ccfba3ce1322ef9 -n 29.txt --connection-string %connection-string%
call az storage container create -n 48ce5c61d5a94897ba48141e8c3bca91 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\30.txt -c 48ce5c61d5a94897ba48141e8c3bca91 -n 30.txt --connection-string %connection-string%
call az storage container create -n b16b07d16bd24f0186f6da16ba08f40d --connection-string %connection-string%
call az storage blob upload -f homework-from-students\31.txt -c b16b07d16bd24f0186f6da16ba08f40d -n 31.txt --connection-string %connection-string%
call az storage container create -n 2ab638ec61464be3b2410dfd321c72b4 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\32.txt -c 2ab638ec61464be3b2410dfd321c72b4 -n 32.txt --connection-string %connection-string%
call az storage container create -n 3ae393fe11f0465baba333ada2d959ac --connection-string %connection-string%
call az storage blob upload -f homework-from-students\33.txt -c 3ae393fe11f0465baba333ada2d959ac -n 33.txt --connection-string %connection-string%
call az storage container create -n 790f9fa7aa5048689d78c11f3b074173 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\34.txt -c 790f9fa7aa5048689d78c11f3b074173 -n 34.txt --connection-string %connection-string%
call az storage container create -n 70422ae49a6b42b289ba955549176a9e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\35.txt -c 70422ae49a6b42b289ba955549176a9e -n 35.txt --connection-string %connection-string%
call az storage container create -n 4a7149e93c7e47739d83d81ee80f3674 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\36.txt -c 4a7149e93c7e47739d83d81ee80f3674 -n 36.txt --connection-string %connection-string%
call az storage container create -n cb935eb3bb1b4da39878feaf0e6b7dfe --connection-string %connection-string%
call az storage blob upload -f homework-from-students\37.txt -c cb935eb3bb1b4da39878feaf0e6b7dfe -n 37.txt --connection-string %connection-string%
call az storage container create -n 9a0cb583e6584c929743ca1117cb7a9f --connection-string %connection-string%
call az storage blob upload -f homework-from-students\38.txt -c 9a0cb583e6584c929743ca1117cb7a9f -n 38.txt --connection-string %connection-string%
call az storage container create -n d221a381c0d844929c09f50c911c3187 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\39.txt -c d221a381c0d844929c09f50c911c3187 -n 39.txt --connection-string %connection-string%
call az storage container create -n 5d1d9bb0dc624f679e8649926f0f3c86 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\40.txt -c 5d1d9bb0dc624f679e8649926f0f3c86 -n 40.txt --connection-string %connection-string%
call az storage container create -n ef0503f068c54627b947fb829b691ab4 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\41.txt -c ef0503f068c54627b947fb829b691ab4 -n 41.txt --connection-string %connection-string%
call az storage container create -n 28558a14a6e347afbd808b8402b02435 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\42.txt -c 28558a14a6e347afbd808b8402b02435 -n 42.txt --connection-string %connection-string%
call az storage container create -n 2368b5e8077f4472b4416353dd39515c --connection-string %connection-string%
call az storage blob upload -f homework-from-students\43.txt -c 2368b5e8077f4472b4416353dd39515c -n 43.txt --connection-string %connection-string%
call az storage container create -n 339df528077c4bb3b77b6405b7d5f95e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\44.txt -c 339df528077c4bb3b77b6405b7d5f95e -n 44.txt --connection-string %connection-string%
call az storage container create -n 2443597a1296407ba501d87f98093c61 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\45.txt -c 2443597a1296407ba501d87f98093c61 -n 45.txt --connection-string %connection-string%
call az storage container create -n 142375090fc745c79dd70ec61c16baec --connection-string %connection-string%
call az storage blob upload -f homework-from-students\46.txt -c 142375090fc745c79dd70ec61c16baec -n 46.txt --connection-string %connection-string%
call az storage container create -n e50c650a89a54059a9d519862ea10e75 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\47.txt -c e50c650a89a54059a9d519862ea10e75 -n 47.txt --connection-string %connection-string%
call az storage container create -n 3d50c240b0bd42bebde3c80af8a8ea97 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\48.txt -c 3d50c240b0bd42bebde3c80af8a8ea97 -n 48.txt --connection-string %connection-string%
call az storage container create -n 23ef7deb92ce4abfbd58fe816de1c019 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\49.txt -c 23ef7deb92ce4abfbd58fe816de1c019 -n 49.txt --connection-string %connection-string%
call az storage container create -n 4ead95e7cdbf4f8d8a5fbc877f25cd46 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\50.txt -c 4ead95e7cdbf4f8d8a5fbc877f25cd46 -n 50.txt --connection-string %connection-string%
call az storage container create -n 5af01b0502d043109760dd77525eb2b4 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\51.txt -c 5af01b0502d043109760dd77525eb2b4 -n 51.txt --connection-string %connection-string%
call az storage container create -n 0d77ec1b3336417898790516cb67f0a7 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\52.txt -c 0d77ec1b3336417898790516cb67f0a7 -n 52.txt --connection-string %connection-string%
call az storage container create -n 9b8ab0ad17404a0985c8ff29cad0fd5c --connection-string %connection-string%
call az storage blob upload -f homework-from-students\53.txt -c 9b8ab0ad17404a0985c8ff29cad0fd5c -n 53.txt --connection-string %connection-string%
call az storage container create -n fcca8b7bd858454fa408e7d90e0df47e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\54.txt -c fcca8b7bd858454fa408e7d90e0df47e -n 54.txt --connection-string %connection-string%
call az storage container create -n e7337fe6e70e439780f89bc63f53431b --connection-string %connection-string%
call az storage blob upload -f homework-from-students\55.txt -c e7337fe6e70e439780f89bc63f53431b -n 55.txt --connection-string %connection-string%
call az storage container create -n 3d2086de5f8c4729b3de758764d02858 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\56.txt -c 3d2086de5f8c4729b3de758764d02858 -n 56.txt --connection-string %connection-string%
call az storage container create -n 9cc69a501fe34bc1b82cde49567700f3 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\57.txt -c 9cc69a501fe34bc1b82cde49567700f3 -n 57.txt --connection-string %connection-string%
call az storage container create -n 3b1cd5104b754afeb0038521763585c6 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\58.txt -c 3b1cd5104b754afeb0038521763585c6 -n 58.txt --connection-string %connection-string%
call az storage container create -n cab628f46b5b4be7af461b16331b8655 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\59.txt -c cab628f46b5b4be7af461b16331b8655 -n 59.txt --connection-string %connection-string%
call az storage container create -n da52f8faf5524b37bae546432b31059a --connection-string %connection-string%
call az storage blob upload -f homework-from-students\60.txt -c da52f8faf5524b37bae546432b31059a -n 60.txt --connection-string %connection-string%
call az storage container create -n f15d723176404f7890cf1b450eaa3f22 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\61.txt -c f15d723176404f7890cf1b450eaa3f22 -n 61.txt --connection-string %connection-string%
call az storage container create -n de77986245f74784aa9a9592190e4652 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\62.txt -c de77986245f74784aa9a9592190e4652 -n 62.txt --connection-string %connection-string%
call az storage container create -n 40055496ea874b20bcc91af7ac31e097 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\63.txt -c 40055496ea874b20bcc91af7ac31e097 -n 63.txt --connection-string %connection-string%
call az storage container create -n fba330a3bbea4093943f557816bfe433 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\64.txt -c fba330a3bbea4093943f557816bfe433 -n 64.txt --connection-string %connection-string%
call az storage container create -n a6f640954bce43c7ab632b7e320ac789 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\65.txt -c a6f640954bce43c7ab632b7e320ac789 -n 65.txt --connection-string %connection-string%
call az storage container create -n 98c6214ff8dc491197be59b438d7abed --connection-string %connection-string%
call az storage blob upload -f homework-from-students\66.txt -c 98c6214ff8dc491197be59b438d7abed -n 66.txt --connection-string %connection-string%
call az storage container create -n 41f09f385b9b4fe7a041aa02ee85fd63 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\67.txt -c 41f09f385b9b4fe7a041aa02ee85fd63 -n 67.txt --connection-string %connection-string%
call az storage container create -n 5987fffac93f4be3adc4ffb1b61e6292 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\68.txt -c 5987fffac93f4be3adc4ffb1b61e6292 -n 68.txt --connection-string %connection-string%
call az storage container create -n 81a7ee5597bd46148473a5d3f513829f --connection-string %connection-string%
call az storage blob upload -f homework-from-students\69.txt -c 81a7ee5597bd46148473a5d3f513829f -n 69.txt --connection-string %connection-string%
call az storage container create -n da6b7eb7719f40f1acd759b8b24301e1 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\70.txt -c da6b7eb7719f40f1acd759b8b24301e1 -n 70.txt --connection-string %connection-string%
call az storage container create -n 70d67eb9f0b4403491ff837b86e851de --connection-string %connection-string%
call az storage blob upload -f homework-from-students\71.txt -c 70d67eb9f0b4403491ff837b86e851de -n 71.txt --connection-string %connection-string%
call az storage container create -n 5b4ba0f2419b469faf08ef9a291b2f76 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\72.txt -c 5b4ba0f2419b469faf08ef9a291b2f76 -n 72.txt --connection-string %connection-string%
call az storage container create -n 0626482b5ba248c69bf6cb3c26ec2ace --connection-string %connection-string%
call az storage blob upload -f homework-from-students\73.txt -c 0626482b5ba248c69bf6cb3c26ec2ace -n 73.txt --connection-string %connection-string%
call az storage container create -n 26034630e6fb48bb830f78c578d695cf --connection-string %connection-string%
call az storage blob upload -f homework-from-students\74.txt -c 26034630e6fb48bb830f78c578d695cf -n 74.txt --connection-string %connection-string%
call az storage container create -n f2924e0f066f4461856b717654c7b32f --connection-string %connection-string%
call az storage blob upload -f homework-from-students\75.txt -c f2924e0f066f4461856b717654c7b32f -n 75.txt --connection-string %connection-string%
call az storage container create -n b5f1d0ba33d14ec08b1c224e22e105fd --connection-string %connection-string%
call az storage blob upload -f homework-from-students\76.txt -c b5f1d0ba33d14ec08b1c224e22e105fd -n 76.txt --connection-string %connection-string%
call az storage container create -n 4416d38946434793bb44672f73723ac1 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\77.txt -c 4416d38946434793bb44672f73723ac1 -n 77.txt --connection-string %connection-string%
call az storage container create -n d0e0279097084f0eb3a2c5326436c47d --connection-string %connection-string%
call az storage blob upload -f homework-from-students\78.txt -c d0e0279097084f0eb3a2c5326436c47d -n 78.txt --connection-string %connection-string%
call az storage container create -n 29c07153d903482a87f1f0f7238472bb --connection-string %connection-string%
call az storage blob upload -f homework-from-students\79.txt -c 29c07153d903482a87f1f0f7238472bb -n 79.txt --connection-string %connection-string%
call az storage container create -n 6fb814c7fa0b4e6984a7ac8fe851d1a5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\80.txt -c 6fb814c7fa0b4e6984a7ac8fe851d1a5 -n 80.txt --connection-string %connection-string%
call az storage container create -n dffdea82df2040169335985fd2689bb5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\81.txt -c dffdea82df2040169335985fd2689bb5 -n 81.txt --connection-string %connection-string%
call az storage container create -n fd0d66c7569e44c08a0dbf18798f1eb3 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\82.txt -c fd0d66c7569e44c08a0dbf18798f1eb3 -n 82.txt --connection-string %connection-string%
call az storage container create -n e91682da215d4c1bbedf4d19f86c705a --connection-string %connection-string%
call az storage blob upload -f homework-from-students\83.txt -c e91682da215d4c1bbedf4d19f86c705a -n 83.txt --connection-string %connection-string%
call az storage container create -n dc247e4b14384bc5bc0de4ae3b31b281 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\84.txt -c dc247e4b14384bc5bc0de4ae3b31b281 -n 84.txt --connection-string %connection-string%
call az storage container create -n aeae2ad0dfe74a93843453ba026e2cc2 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\85.txt -c aeae2ad0dfe74a93843453ba026e2cc2 -n 85.txt --connection-string %connection-string%
call az storage container create -n e3127ae68deb44149d7c63f760bb0459 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\86.txt -c e3127ae68deb44149d7c63f760bb0459 -n 86.txt --connection-string %connection-string%
call az storage container create -n 42de17178f694732b2c1b99639da6dc5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\87.txt -c 42de17178f694732b2c1b99639da6dc5 -n 87.txt --connection-string %connection-string%
call az storage container create -n 423d133e3d2f4f24b1fb3632fb719e68 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\88.txt -c 423d133e3d2f4f24b1fb3632fb719e68 -n 88.txt --connection-string %connection-string%
call az storage container create -n 0ce8bfd0f2c04e34b9a86bf6ccbba05e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\89.txt -c 0ce8bfd0f2c04e34b9a86bf6ccbba05e -n 89.txt --connection-string %connection-string%
call az storage container create -n 8fbc83c29772486091771a643c60d9cc --connection-string %connection-string%
call az storage blob upload -f homework-from-students\90.txt -c 8fbc83c29772486091771a643c60d9cc -n 90.txt --connection-string %connection-string%
call az storage container create -n 9fb8d60ff8ec4fa796c03f30a3280d22 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\91.txt -c 9fb8d60ff8ec4fa796c03f30a3280d22 -n 91.txt --connection-string %connection-string%
call az storage container create -n 83c121f5942346e68f0f4c697583efbd --connection-string %connection-string%
call az storage blob upload -f homework-from-students\92.txt -c 83c121f5942346e68f0f4c697583efbd -n 92.txt --connection-string %connection-string%
call az storage container create -n 5b6be044420245d3a92480fd6418a539 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\93.txt -c 5b6be044420245d3a92480fd6418a539 -n 93.txt --connection-string %connection-string%
call az storage container create -n 5ca6be815bd744d689c8f0094d82b282 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\94.txt -c 5ca6be815bd744d689c8f0094d82b282 -n 94.txt --connection-string %connection-string%
call az storage container create -n 5968b4f5272a4172b4b88e569aa159e2 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\95.txt -c 5968b4f5272a4172b4b88e569aa159e2 -n 95.txt --connection-string %connection-string%
call az storage container create -n e277f038231b4261a5dee0fd73ac7a11 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\96.txt -c e277f038231b4261a5dee0fd73ac7a11 -n 96.txt --connection-string %connection-string%
call az storage container create -n c10ee3352ee14471b219d9ea31667cc9 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\97.txt -c c10ee3352ee14471b219d9ea31667cc9 -n 97.txt --connection-string %connection-string%
call az storage container create -n 5cfa953695504a9a99b7febbf00527ec --connection-string %connection-string%
call az storage blob upload -f homework-from-students\98.txt -c 5cfa953695504a9a99b7febbf00527ec -n 98.txt --connection-string %connection-string%
call az storage container create -n c3eca67980774d8395eddaab04618a31 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\99.txt -c c3eca67980774d8395eddaab04618a31 -n 99.txt --connection-string %connection-string%
call az storage container create -n 3e0f40cd6c1141f38fc5a563e9ba4dad --connection-string %connection-string%
call az storage blob upload -f homework-from-students\100.txt -c 3e0f40cd6c1141f38fc5a563e9ba4dad -n 100.txt --connection-string %connection-string%
call az storage container create -n 8c4c4332427c4c399f16c330eb7fca02 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\101.txt -c 8c4c4332427c4c399f16c330eb7fca02 -n 101.txt --connection-string %connection-string%
call az storage container create -n 9216b6b7720c49cc9978eb7c7f6deb50 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\102.txt -c 9216b6b7720c49cc9978eb7c7f6deb50 -n 102.txt --connection-string %connection-string%
call az storage container create -n c59f953325b5441ebd90de5f2fbdf5c5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\103.txt -c c59f953325b5441ebd90de5f2fbdf5c5 -n 103.txt --connection-string %connection-string%
call az storage container create -n 922ad2ebf95b46c68a0b12eef4d15bbb --connection-string %connection-string%
call az storage blob upload -f homework-from-students\104.txt -c 922ad2ebf95b46c68a0b12eef4d15bbb -n 104.txt --connection-string %connection-string%
call az storage container create -n 3839be6008f6412e9144b7079ac19e31 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\105.txt -c 3839be6008f6412e9144b7079ac19e31 -n 105.txt --connection-string %connection-string%
call az storage container create -n 5b70b146298a42ba9ca0e66af5126462 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\106.txt -c 5b70b146298a42ba9ca0e66af5126462 -n 106.txt --connection-string %connection-string%
call az storage container create -n 7f0a8997fe894e598497ebacbbdb9b2e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\107.txt -c 7f0a8997fe894e598497ebacbbdb9b2e -n 107.txt --connection-string %connection-string%
call az storage container create -n 3ed2aa410b1541d681d31d1e2f04b255 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\108.txt -c 3ed2aa410b1541d681d31d1e2f04b255 -n 108.txt --connection-string %connection-string%
call az storage container create -n 7ec17ef7b62d4f00a2abee0cfda0ef77 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\109.txt -c 7ec17ef7b62d4f00a2abee0cfda0ef77 -n 109.txt --connection-string %connection-string%
call az storage container create -n 0cdb339767304017b9c16d998904bd1f --connection-string %connection-string%
call az storage blob upload -f homework-from-students\110.txt -c 0cdb339767304017b9c16d998904bd1f -n 110.txt --connection-string %connection-string%
call az storage container create -n aa7077d025b043029133fa80b93cfb57 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\111.txt -c aa7077d025b043029133fa80b93cfb57 -n 111.txt --connection-string %connection-string%
call az storage container create -n 5732f4f7b7394ae78229704ea2cd4b20 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\112.txt -c 5732f4f7b7394ae78229704ea2cd4b20 -n 112.txt --connection-string %connection-string%
call az storage container create -n 3af7825b4cc94cd280c9411199545d37 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\113.txt -c 3af7825b4cc94cd280c9411199545d37 -n 113.txt --connection-string %connection-string%
call az storage container create -n f74e4f152bc94432ac05aca4eb3ca973 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\114.txt -c f74e4f152bc94432ac05aca4eb3ca973 -n 114.txt --connection-string %connection-string%
call az storage container create -n da00ed7efdc94e92851153d9b967c103 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\115.txt -c da00ed7efdc94e92851153d9b967c103 -n 115.txt --connection-string %connection-string%
call az storage container create -n e75474a697d34a908174a55b6f15e448 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\116.txt -c e75474a697d34a908174a55b6f15e448 -n 116.txt --connection-string %connection-string%
call az storage container create -n 324f55368df641adab8ba6eb80458f96 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\117.txt -c 324f55368df641adab8ba6eb80458f96 -n 117.txt --connection-string %connection-string%
call az storage container create -n f66a222d88db4c4da0f2817f1ed874bd --connection-string %connection-string%
call az storage blob upload -f homework-from-students\118.txt -c f66a222d88db4c4da0f2817f1ed874bd -n 118.txt --connection-string %connection-string%
call az storage container create -n 6a63235848dd465eb860f6179cf57a73 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\119.txt -c 6a63235848dd465eb860f6179cf57a73 -n 119.txt --connection-string %connection-string%
call az storage container create -n 782374435aa642cb9dbc3a4c194476cd --connection-string %connection-string%
call az storage blob upload -f homework-from-students\120.txt -c 782374435aa642cb9dbc3a4c194476cd -n 120.txt --connection-string %connection-string%
call az storage container create -n 080d76718ab5481b94ce33e078ffc365 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\121.txt -c 080d76718ab5481b94ce33e078ffc365 -n 121.txt --connection-string %connection-string%
call az storage container create -n ebb3b244b4c44580b50e92ae16c5a65e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\122.txt -c ebb3b244b4c44580b50e92ae16c5a65e -n 122.txt --connection-string %connection-string%
call az storage container create -n e1ec514154794e81ab626017a4362ae5 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\123.txt -c e1ec514154794e81ab626017a4362ae5 -n 123.txt --connection-string %connection-string%
call az storage container create -n 09485c3795ac4079ba99c6dba244639b --connection-string %connection-string%
call az storage blob upload -f homework-from-students\124.txt -c 09485c3795ac4079ba99c6dba244639b -n 124.txt --connection-string %connection-string%
call az storage container create -n 3d99458d27284af59a1fa6f6c25aa8d3 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\125.txt -c 3d99458d27284af59a1fa6f6c25aa8d3 -n 125.txt --connection-string %connection-string%
call az storage container create -n 5b9897b5bb974d27b9c38284e8762ec6 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\126.txt -c 5b9897b5bb974d27b9c38284e8762ec6 -n 126.txt --connection-string %connection-string%
call az storage container create -n c997def9a24c420ea0a3093e9c1e14ec --connection-string %connection-string%
call az storage blob upload -f homework-from-students\127.txt -c c997def9a24c420ea0a3093e9c1e14ec -n 127.txt --connection-string %connection-string%
call az storage container create -n f3b63e8a65b24154b77a24e418765b27 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\128.txt -c f3b63e8a65b24154b77a24e418765b27 -n 128.txt --connection-string %connection-string%
call az storage container create -n 97393415636c46e1a45781e9f13748d1 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\129.txt -c 97393415636c46e1a45781e9f13748d1 -n 129.txt --connection-string %connection-string%
call az storage container create -n 0f258706dcbc4fab8c8350bb1260ee7b --connection-string %connection-string%
call az storage blob upload -f homework-from-students\130.txt -c 0f258706dcbc4fab8c8350bb1260ee7b -n 130.txt --connection-string %connection-string%
call az storage container create -n 4707ebe0ab224774b4ec0f42bfa36b53 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\131.txt -c 4707ebe0ab224774b4ec0f42bfa36b53 -n 131.txt --connection-string %connection-string%
call az storage container create -n c3d0416402074288bd86a7e2583ccbb9 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\132.txt -c c3d0416402074288bd86a7e2583ccbb9 -n 132.txt --connection-string %connection-string%
call az storage container create -n da58b5a1b3ba4693bdec528e332d6059 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\133.txt -c da58b5a1b3ba4693bdec528e332d6059 -n 133.txt --connection-string %connection-string%
call az storage container create -n 4955ece4118d4336a2ea182b29a61217 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\134.txt -c 4955ece4118d4336a2ea182b29a61217 -n 134.txt --connection-string %connection-string%
call az storage container create -n 3bf0c117c7884c95a4f4bb0327409c33 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\135.txt -c 3bf0c117c7884c95a4f4bb0327409c33 -n 135.txt --connection-string %connection-string%
call az storage container create -n 9ad1f107038246feba15332c222dc156 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\136.txt -c 9ad1f107038246feba15332c222dc156 -n 136.txt --connection-string %connection-string%
call az storage container create -n a6a72f334da1446ebe9336a9c22304f7 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\137.txt -c a6a72f334da1446ebe9336a9c22304f7 -n 137.txt --connection-string %connection-string%
call az storage container create -n f3e3c4e26cf546b8bb2c1a214dc938cf --connection-string %connection-string%
call az storage blob upload -f homework-from-students\138.txt -c f3e3c4e26cf546b8bb2c1a214dc938cf -n 138.txt --connection-string %connection-string%
call az storage container create -n 7be1dd734626438fbce5ec6ba263cf35 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\139.txt -c 7be1dd734626438fbce5ec6ba263cf35 -n 139.txt --connection-string %connection-string%
call az storage container create -n ca5c413708ee4cd8b81c92fb87079e11 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\140.txt -c ca5c413708ee4cd8b81c92fb87079e11 -n 140.txt --connection-string %connection-string%
call az storage container create -n 959be77036254773afb4ce53b7d6a749 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\141.txt -c 959be77036254773afb4ce53b7d6a749 -n 141.txt --connection-string %connection-string%
call az storage container create -n 098a35a2c7284bd8b7c69c379d126821 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\142.txt -c 098a35a2c7284bd8b7c69c379d126821 -n 142.txt --connection-string %connection-string%
call az storage container create -n e390a573d81947b7a0233ab6148a3c57 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\143.txt -c e390a573d81947b7a0233ab6148a3c57 -n 143.txt --connection-string %connection-string%
call az storage container create -n 1bc0cf62d64c41f2b2d7b51523a2bb01 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\144.txt -c 1bc0cf62d64c41f2b2d7b51523a2bb01 -n 144.txt --connection-string %connection-string%
call az storage container create -n 55434b22f0a74a5eb0b70a00e847bb97 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\145.txt -c 55434b22f0a74a5eb0b70a00e847bb97 -n 145.txt --connection-string %connection-string%
call az storage container create -n 7fd6a8f110994acbb0b54b82f04046eb --connection-string %connection-string%
call az storage blob upload -f homework-from-students\146.txt -c 7fd6a8f110994acbb0b54b82f04046eb -n 146.txt --connection-string %connection-string%
call az storage container create -n 0c315466bd4844d98b74d94068b0e942 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\147.txt -c 0c315466bd4844d98b74d94068b0e942 -n 147.txt --connection-string %connection-string%
call az storage container create -n bcc10df9c3464b2f8cd83a0899866208 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\148.txt -c bcc10df9c3464b2f8cd83a0899866208 -n 148.txt --connection-string %connection-string%
call az storage container create -n 157e963688c54e6b9d19fefba7dfb0c6 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\149.txt -c 157e963688c54e6b9d19fefba7dfb0c6 -n 149.txt --connection-string %connection-string%
call az storage container create -n cc58efed683f4af68e5f4bc041eed6a4 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\150.txt -c cc58efed683f4af68e5f4bc041eed6a4 -n 150.txt --connection-string %connection-string%
call az storage container create -n 82df517f64674590865103bdab3d08b7 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\151.txt -c 82df517f64674590865103bdab3d08b7 -n 151.txt --connection-string %connection-string%
call az storage container create -n 033bf5392d83465191de92714c505667 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\152.txt -c 033bf5392d83465191de92714c505667 -n 152.txt --connection-string %connection-string%
call az storage container create -n 2268f5a52d8644578281115acb62a80f --connection-string %connection-string%
call az storage blob upload -f homework-from-students\153.txt -c 2268f5a52d8644578281115acb62a80f -n 153.txt --connection-string %connection-string%
call az storage container create -n 0f0c58966c294fd1855cfee963cfc5fa --connection-string %connection-string%
call az storage blob upload -f homework-from-students\154.txt -c 0f0c58966c294fd1855cfee963cfc5fa -n 154.txt --connection-string %connection-string%
call az storage container create -n 939744731f584eb9ad1555f262d5870e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\155.txt -c 939744731f584eb9ad1555f262d5870e -n 155.txt --connection-string %connection-string%
call az storage container create -n 342cce0fff47466fab4af2afa77aae1d --connection-string %connection-string%
call az storage blob upload -f homework-from-students\156.txt -c 342cce0fff47466fab4af2afa77aae1d -n 156.txt --connection-string %connection-string%
call az storage container create -n 6a0c13d7c2f04bd38bb29fb3613ca821 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\157.txt -c 6a0c13d7c2f04bd38bb29fb3613ca821 -n 157.txt --connection-string %connection-string%
call az storage container create -n 4a97a6319a8e487a9e6f8eba88dcfb26 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\158.txt -c 4a97a6319a8e487a9e6f8eba88dcfb26 -n 158.txt --connection-string %connection-string%
call az storage container create -n 0e9703b5e53141f3866592a30555c7a7 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\159.txt -c 0e9703b5e53141f3866592a30555c7a7 -n 159.txt --connection-string %connection-string%
call az storage container create -n 5e089d99f0c543af9f68cb379363711a --connection-string %connection-string%
call az storage blob upload -f homework-from-students\160.txt -c 5e089d99f0c543af9f68cb379363711a -n 160.txt --connection-string %connection-string%
call az storage container create -n efd65a5c7df04db791bd6481ec363f98 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\161.txt -c efd65a5c7df04db791bd6481ec363f98 -n 161.txt --connection-string %connection-string%
call az storage container create -n 5c4bb927b1ec46ca8f3fb6dfeedbaac8 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\162.txt -c 5c4bb927b1ec46ca8f3fb6dfeedbaac8 -n 162.txt --connection-string %connection-string%
call az storage container create -n d80bb0db7d134e8380188e923b49a0da --connection-string %connection-string%
call az storage blob upload -f homework-from-students\163.txt -c d80bb0db7d134e8380188e923b49a0da -n 163.txt --connection-string %connection-string%
call az storage container create -n ea87b7b3b438431d96e4b27bd9b0dcdf --connection-string %connection-string%
call az storage blob upload -f homework-from-students\164.txt -c ea87b7b3b438431d96e4b27bd9b0dcdf -n 164.txt --connection-string %connection-string%
call az storage container create -n 0add23d6763f43759401ab6c50068c82 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\165.txt -c 0add23d6763f43759401ab6c50068c82 -n 165.txt --connection-string %connection-string%
call az storage container create -n d43564d9250945af9b436a2181b3cc95 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\166.txt -c d43564d9250945af9b436a2181b3cc95 -n 166.txt --connection-string %connection-string%
call az storage container create -n 654313e129254612812e8e593bbe0b9e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\167.txt -c 654313e129254612812e8e593bbe0b9e -n 167.txt --connection-string %connection-string%
call az storage container create -n f6af4bb67bcb4ddba9188e5f22978deb --connection-string %connection-string%
call az storage blob upload -f homework-from-students\168.txt -c f6af4bb67bcb4ddba9188e5f22978deb -n 168.txt --connection-string %connection-string%
call az storage container create -n 5b6dc48995904c0793239be7f7df1bae --connection-string %connection-string%
call az storage blob upload -f homework-from-students\169.txt -c 5b6dc48995904c0793239be7f7df1bae -n 169.txt --connection-string %connection-string%
call az storage container create -n 8321091b0e7340f08ca296d78d9f8c1b --connection-string %connection-string%
call az storage blob upload -f homework-from-students\170.txt -c 8321091b0e7340f08ca296d78d9f8c1b -n 170.txt --connection-string %connection-string%
call az storage container create -n d001c618d2474dd9b9cf59e280f0adea --connection-string %connection-string%
call az storage blob upload -f homework-from-students\171.txt -c d001c618d2474dd9b9cf59e280f0adea -n 171.txt --connection-string %connection-string%
call az storage container create -n 1f0d7e9d7d354fddaef25ff27d301a71 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\172.txt -c 1f0d7e9d7d354fddaef25ff27d301a71 -n 172.txt --connection-string %connection-string%
call az storage container create -n ad5f8c82e8504eef94dce7c66fdadab8 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\173.txt -c ad5f8c82e8504eef94dce7c66fdadab8 -n 173.txt --connection-string %connection-string%
call az storage container create -n 73b2181e34ad4c7383f2975768337fdf --connection-string %connection-string%
call az storage blob upload -f homework-from-students\174.txt -c 73b2181e34ad4c7383f2975768337fdf -n 174.txt --connection-string %connection-string%
call az storage container create -n 3865bc35ce4b4dd38e6ddccf75110927 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\175.txt -c 3865bc35ce4b4dd38e6ddccf75110927 -n 175.txt --connection-string %connection-string%
call az storage container create -n 9f896702d032431585329c5a2e98be09 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\176.txt -c 9f896702d032431585329c5a2e98be09 -n 176.txt --connection-string %connection-string%
call az storage container create -n 81c9bf0a1773444c802062b048b4a3f4 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\177.txt -c 81c9bf0a1773444c802062b048b4a3f4 -n 177.txt --connection-string %connection-string%
call az storage container create -n 86adf6b6c9924ad69e59e6ed8131ce08 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\178.txt -c 86adf6b6c9924ad69e59e6ed8131ce08 -n 178.txt --connection-string %connection-string%
call az storage container create -n 5e05957828f0494d89b1719a3a789d23 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\179.txt -c 5e05957828f0494d89b1719a3a789d23 -n 179.txt --connection-string %connection-string%
call az storage container create -n 2d867e4b674b433495167241db247e48 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\180.txt -c 2d867e4b674b433495167241db247e48 -n 180.txt --connection-string %connection-string%
call az storage container create -n 62ff40f589ed4ca684348e38f893d58e --connection-string %connection-string%
call az storage blob upload -f homework-from-students\181.txt -c 62ff40f589ed4ca684348e38f893d58e -n 181.txt --connection-string %connection-string%
call az storage container create -n 22a565d0e08a4952a94cc15d43b4f704 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\182.txt -c 22a565d0e08a4952a94cc15d43b4f704 -n 182.txt --connection-string %connection-string%
call az storage container create -n 422f3bba9eb5475397d62f33e0362928 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\183.txt -c 422f3bba9eb5475397d62f33e0362928 -n 183.txt --connection-string %connection-string%
call az storage container create -n 8fa1f79bcf2a4711a0d46650601408e3 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\184.txt -c 8fa1f79bcf2a4711a0d46650601408e3 -n 184.txt --connection-string %connection-string%
call az storage container create -n 809e48f3ffa34a67abb71ff7f67ba578 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\185.txt -c 809e48f3ffa34a67abb71ff7f67ba578 -n 185.txt --connection-string %connection-string%
call az storage container create -n 238e618f79fb476fb0932740ef6451e0 --connection-string %connection-string%
call az storage blob upload -f homework-from-students\186.txt -c 238e618f79fb476fb0932740ef6451e0 -n 186.txt --connection-string %connection-string%
pause