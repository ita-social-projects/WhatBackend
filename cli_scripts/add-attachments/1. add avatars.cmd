set connection-string=""DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1""
call az storage container create -n dc2ad5a5e5384173b1729128fa50ff2b --connection-string %connection-string%
call az storage blob upload -f avatars\1.jpg -c dc2ad5a5e5384173b1729128fa50ff2b -n 1.jpg --connection-string %connection-string%
call az storage container create -n da225766510245959b6cd433484fff23 --connection-string %connection-string%
call az storage blob upload -f avatars\2.jpg -c da225766510245959b6cd433484fff23 -n 2.jpg --connection-string %connection-string%
call az storage container create -n c930fd8fc24c4ca1a25bc0883481a81f --connection-string %connection-string%
call az storage blob upload -f avatars\3.jpg -c c930fd8fc24c4ca1a25bc0883481a81f -n 3.jpg --connection-string %connection-string%
call az storage container create -n be76c233ff764eed94bd693923b633a2 --connection-string %connection-string%
call az storage blob upload -f avatars\4.jpg -c be76c233ff764eed94bd693923b633a2 -n 4.jpg --connection-string %connection-string%
call az storage container create -n 5c352117dab44eadabf54d42d29524dc --connection-string %connection-string%
call az storage blob upload -f avatars\5.jpg -c 5c352117dab44eadabf54d42d29524dc -n 5.jpg --connection-string %connection-string%
call az storage container create -n 24e4fe3f7c634a0093fe0050f01e9894 --connection-string %connection-string%
call az storage blob upload -f avatars\6.jpg -c 24e4fe3f7c634a0093fe0050f01e9894 -n 6.jpg --connection-string %connection-string%
call az storage container create -n 28edbc17f2394a5c967e5f5568bc523a --connection-string %connection-string%
call az storage blob upload -f avatars\7.jpg -c 28edbc17f2394a5c967e5f5568bc523a -n 7.jpg --connection-string %connection-string%
call az storage container create -n 1d9a34a8b9494a57b5bf87fe87a14c1b --connection-string %connection-string%
call az storage blob upload -f avatars\8.jpg -c 1d9a34a8b9494a57b5bf87fe87a14c1b -n 8.jpg --connection-string %connection-string%
call az storage container create -n bd18d83832094662877fdd44ded8ca63 --connection-string %connection-string%
call az storage blob upload -f avatars\9.jpg -c bd18d83832094662877fdd44ded8ca63 -n 9.jpg --connection-string %connection-string%
call az storage container create -n e80c2bd3e48a4d8b8eeb95d41fcb6e52 --connection-string %connection-string%
call az storage blob upload -f avatars\10.jpg -c e80c2bd3e48a4d8b8eeb95d41fcb6e52 -n 10.jpg --connection-string %connection-string%
call az storage container create -n 32f19972bebb42a689104071a11c5e33 --connection-string %connection-string%
call az storage blob upload -f avatars\11.jpg -c 32f19972bebb42a689104071a11c5e33 -n 11.jpg --connection-string %connection-string%
call az storage container create -n b2c214175eb74adabecd2b9747d8dc85 --connection-string %connection-string%
call az storage blob upload -f avatars\12.jpg -c b2c214175eb74adabecd2b9747d8dc85 -n 12.jpg --connection-string %connection-string%
call az storage container create -n fbbfa551fede45f5940ca750a3f7986b --connection-string %connection-string%
call az storage blob upload -f avatars\13.jpg -c fbbfa551fede45f5940ca750a3f7986b -n 13.jpg --connection-string %connection-string%
call az storage container create -n 630846a66d874499b529cb5ad4db82e2 --connection-string %connection-string%
call az storage blob upload -f avatars\14.jpg -c 630846a66d874499b529cb5ad4db82e2 -n 14.jpg --connection-string %connection-string%
call az storage container create -n b1a46fee2f8c4fdbbffa53465641654c --connection-string %connection-string%
call az storage blob upload -f avatars\15.jpg -c b1a46fee2f8c4fdbbffa53465641654c -n 15.jpg --connection-string %connection-string%
call az storage container create -n 1736a6ec4a4845349b38033b613982c9 --connection-string %connection-string%
call az storage blob upload -f avatars\16.jpg -c 1736a6ec4a4845349b38033b613982c9 -n 16.jpg --connection-string %connection-string%
call az storage container create -n ade5fa3adb51431e848e0355cbaaec6c --connection-string %connection-string%
call az storage blob upload -f avatars\17.jpg -c ade5fa3adb51431e848e0355cbaaec6c -n 17.jpg --connection-string %connection-string%
call az storage container create -n 8c1a55663a94456ca3f6c58749bb90a7 --connection-string %connection-string%
call az storage blob upload -f avatars\18.jpg -c 8c1a55663a94456ca3f6c58749bb90a7 -n 18.jpg --connection-string %connection-string%
call az storage container create -n 4bc338e6e98d489dbf170d719c74fe68 --connection-string %connection-string%
call az storage blob upload -f avatars\19.jpg -c 4bc338e6e98d489dbf170d719c74fe68 -n 19.jpg --connection-string %connection-string%
call az storage container create -n 99e8d584c6af4ff793c13b174202cfd6 --connection-string %connection-string%
call az storage blob upload -f avatars\20.jpg -c 99e8d584c6af4ff793c13b174202cfd6 -n 20.jpg --connection-string %connection-string%
call az storage container create -n 4b5d7bf862b24ed0aaad07b5c916de0f --connection-string %connection-string%
call az storage blob upload -f avatars\21.jpg -c 4b5d7bf862b24ed0aaad07b5c916de0f -n 21.jpg --connection-string %connection-string%
call az storage container create -n 17f4f623739b4adcb6d4e5fd1bae377c --connection-string %connection-string%
call az storage blob upload -f avatars\22.jpg -c 17f4f623739b4adcb6d4e5fd1bae377c -n 22.jpg --connection-string %connection-string%
call az storage container create -n f742931ff3f74ff59efdee9433d3ac41 --connection-string %connection-string%
call az storage blob upload -f avatars\23.jpg -c f742931ff3f74ff59efdee9433d3ac41 -n 23.jpg --connection-string %connection-string%
call az storage container create -n 45873fc69dac423db15c04d9ddadee2b --connection-string %connection-string%
call az storage blob upload -f avatars\24.jpg -c 45873fc69dac423db15c04d9ddadee2b -n 24.jpg --connection-string %connection-string%
call az storage container create -n cf512d51ac6b4cd982b3e5039aea664f --connection-string %connection-string%
call az storage blob upload -f avatars\25.jpg -c cf512d51ac6b4cd982b3e5039aea664f -n 25.jpg --connection-string %connection-string%
call az storage container create -n bf5f8faa55ad4faf947b2c7a5b636293 --connection-string %connection-string%
call az storage blob upload -f avatars\26.jpg -c bf5f8faa55ad4faf947b2c7a5b636293 -n 26.jpg --connection-string %connection-string%
call az storage container create -n 4a607d89efe4460580f694e10e23314d --connection-string %connection-string%
call az storage blob upload -f avatars\27.jpg -c 4a607d89efe4460580f694e10e23314d -n 27.jpg --connection-string %connection-string%
call az storage container create -n 5eb6b24a5e054e169c4e7074444ec950 --connection-string %connection-string%
call az storage blob upload -f avatars\28.jpg -c 5eb6b24a5e054e169c4e7074444ec950 -n 28.jpg --connection-string %connection-string%
call az storage container create -n 7646d6d322434e8cbcd29fbf6dd030d2 --connection-string %connection-string%
call az storage blob upload -f avatars\29.jpg -c 7646d6d322434e8cbcd29fbf6dd030d2 -n 29.jpg --connection-string %connection-string%
call az storage container create -n d8e771dec4ab484abf5478285a822d37 --connection-string %connection-string%
call az storage blob upload -f avatars\30.jpg -c d8e771dec4ab484abf5478285a822d37 -n 30.jpg --connection-string %connection-string%
call az storage container create -n 9ee0c7d8e01346ee844ae2893d7e4df7 --connection-string %connection-string%
call az storage blob upload -f avatars\31.jpg -c 9ee0c7d8e01346ee844ae2893d7e4df7 -n 31.jpg --connection-string %connection-string%
call az storage container create -n 672f926896aa4cba996f68c235ddd2d4 --connection-string %connection-string%
call az storage blob upload -f avatars\32.jpg -c 672f926896aa4cba996f68c235ddd2d4 -n 32.jpg --connection-string %connection-string%
call az storage container create -n e7de05778f1e4fd7b208451a94cc56d2 --connection-string %connection-string%
call az storage blob upload -f avatars\33.jpg -c e7de05778f1e4fd7b208451a94cc56d2 -n 33.jpg --connection-string %connection-string%
call az storage container create -n 5558128d1c79495a934793bd0d63b2e2 --connection-string %connection-string%
call az storage blob upload -f avatars\34.jpg -c 5558128d1c79495a934793bd0d63b2e2 -n 34.jpg --connection-string %connection-string%
call az storage container create -n feeaecdd6aa347dfa5d4b0ddda771ec5 --connection-string %connection-string%
call az storage blob upload -f avatars\35.jpg -c feeaecdd6aa347dfa5d4b0ddda771ec5 -n 35.jpg --connection-string %connection-string%
call az storage container create -n 30c4b294f96e41b5b29b3709deb0b3e0 --connection-string %connection-string%
call az storage blob upload -f avatars\36.jpg -c 30c4b294f96e41b5b29b3709deb0b3e0 -n 36.jpg --connection-string %connection-string%
call az storage container create -n 5680a096834f40f6b7ad6292151657bc --connection-string %connection-string%
call az storage blob upload -f avatars\37.jpg -c 5680a096834f40f6b7ad6292151657bc -n 37.jpg --connection-string %connection-string%
call az storage container create -n 70a21d6d5fb4467c81b812dc89ec3e58 --connection-string %connection-string%
call az storage blob upload -f avatars\38.jpg -c 70a21d6d5fb4467c81b812dc89ec3e58 -n 38.jpg --connection-string %connection-string%
call az storage container create -n 5a7bfffeeb9d47d68c1599eb68e32eae --connection-string %connection-string%
call az storage blob upload -f avatars\39.jpg -c 5a7bfffeeb9d47d68c1599eb68e32eae -n 39.jpg --connection-string %connection-string%
call az storage container create -n db23452c773a4ec6a429f91538841d7a --connection-string %connection-string%
call az storage blob upload -f avatars\40.jpg -c db23452c773a4ec6a429f91538841d7a -n 40.jpg --connection-string %connection-string%
pause