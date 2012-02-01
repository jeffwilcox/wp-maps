# JeffWilcox.Maps
One of the nice visuals of a Windows Phone application with mapping display of a simple area map whenever you view a place in the app.
 
Unfortunately, most map controls, such as the Bing Maps control included with the platform, are highly sophisticated: they let you have a bunch of pushpins, gather information about bounding boxes, etc. This awesome feature set can have a negative effect on performance.

![Sample Screenshot with JeffWilcox.Maps](http://www.jeff.wilcox.name/wp-content/uploads/2012/01/SampleMap_thumb.png)
 
My static map control is simple but nice because it’s an easy replacement, can be used in Panorama/Pivots, and overall still has a great, high-speed experience that your users will enjoy.

# Details
Details of the library are in this [blog post](http://www.jeff.wilcox.name/2012/01/jeffwilcox-maps/) about the control.  Here are some of the basics:

* Supports Bing, Google, Mapquest and OpenStreetMap
* Gets a static map image (type based on provider) and displays a pushpin in the center
* Maps are provider-based if you have a special implementation you need
* Easy to use in markup and supports data binding properties

# Credits
* [Jeff Wilcox](http://www.jeff.wilcox.name) ([@jeffwilcox](http://twitter.com/jeffwilcox)) for the creation of this control
* [Tim Heuer](http://timheuer.com/blog/) ([@timheuer](http://twitter.com/timheuer)) for adding MapQuest and OpenStreetMap
