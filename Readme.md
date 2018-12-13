# Christmas Kata 2018

[This is the file that needs tests adding](ChristmasKata2018/CustomLetterHandlerFactory.cs)

This kata is about refactoring to make something easy.
No use of MOQ allowed.

There is a simple way.

Feel free to solve this in anyway you wish. This was taken from a live example.
I have taken some of the code from that to attempt to replicate its 
complexities. 

This kata started as we wanted to get more examples of possible ways to
factor this to make it testable. It was felt that the way we found was a bit meh.
As a result I ran a couple of katas on it - but nobody managed to make it testable in an hour.

So I thought let's see how the XP bods do!

## TLDR

Put tests around the above file to bring out its current specification


## Background

The elves need to update their home rolled ERP (Elven Resource Planning) system.

Santa feels it is time to return to roots and be able to respond to letters to 
any of his hundreds of names. However due to difficulty of reading 
all these (an ongoing eye condition) he feels it will be necessary to extend
the existing functionality to accomodate these.

Due to the secrecy of the elven systems they will not give us their code.
Also due to a lack of time they are unwilling to even provide a working 
harness. So we have the extremities of a system, no idea about how it is
really used and hooked up but a requirement to make a change.

At least soon it will be a different year!

## Integration informatioon
The part of the process we have been made aware of though is that there 
is some kind of letter classifier plugged in the front that is forwarding
letters to the CustomerLetterHandlerFactory which is then responsible for
producing the correct type of letter handler for that name + current config combination

## The goal
**Naturally there are no tests - this needs to be addressed** as we know there
are a lot of new requirements coming into this area. 

Moreover changes to other parts of the system are likely to impact the user experience.


Should anyone manage this then there are the following requirements.

The requirements are
1. We now need to be able to optionally provide overriding letter handlers
for  Mother Christmas. Mother Christmas is a name of his that involves 
people sending santa gifts for other people. In terms of a change, if 
'mother christmas' is optionally active for a letter origin then letter handlers
with the string MotherChristmas in their names will override the base 
letter handler. We are not concerned with the actual behaviour of the 
handler here
                                  
2. If the letter origin is Yorkshire and the present tier is set to coal
then a special MontyPythonLetterHandler should be used. Moreover we know
there are going to be many more Christmas Eggs such as this.
