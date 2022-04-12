# SayolloSDK-ZemerAltitzer
This is an example project for Sayollo

In order to use the SDK, download the project as zip and extract the folder SayolloSDK.
Drag or paste the folder into your project's Assets folder.

In order to create new "Video Ad spots"  or new ItemPurchase views, click the "Sayollo" menu
("located next to the component menu") and choose either of the options.
A new GameObject will be added to your hirarchy.
----------------------------------------------------------------------------------------

I tried to decouple the classes as much as possible. 

The purchase view (presented as an overlay) is comprised of 3 elements:
1)User input reader - responsible for reading user input and manipulation it into json format.
2)Request handler - responsible for handling requests to the server
3)Item view - responsible for activating/deactivating ui panels.

The video ad (presented in world space so it can be used on billboards) is comprised of 2 elements:
1)Video handler - responsible for preparing the video and playing it
2)Request handler - responsible for handling requests to the server and downloading 
the video - if nessecary.

The only design pattern I ended up using is the "Observer Pattern" which allowed me to decouple 
the classes using events.

Some of the functionallity required the use of async programming.
For server requests - I used an IEnumerator (Couroutine) as shown in the 
manual (my intuition however was to use an async function to avoid cluttering the main thread with the server requests).
For other cases I mostly used async/await and event invocation.


If you have any questions please contact me!
Thank you for your time!

Zemer Altitzer
052-4735888
zem.gamez1@gmail.com
