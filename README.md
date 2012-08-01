NerdDinnerRevolutions
=====================

A rework of the NerdDinnerReloaded sln to use Backbone and NancyFx

- Mostly kept the original EF classes intact, as this was more to show how to move from an existing asp.net mvc3 app to backbone and nancyfx - but I did move them to a separate .Data project

## NancyFX changes
- Created separated GET and POST endpoints for getting a list of dinners and creating a dinner respectively
- When getting a list of dinners, EF returns "DynamicProxy" classes which don't serialize to JSON very well because of circular references.  Have to use linq to map results to anonymous types.  Unfortunate but necessary.
- When creating a new dinner, I'm using Request.Form, which forces me to submit the data as a application/x-www-form-urlencoded, which also requires me to set Backbone.emulateJSON = true in the JS.  Works well though.  Would like to see a Request.Body.AsJson or some clever way of automagically parsing Request.Body. (can be done via extension method)
- When creating a new dinner, I left the DataAnnotation attributes on the .Data classes intact, so attempting to save via .SaveChanges can throw DbEntityValidationException messages.  This is where validation will come in the future.  Note how convoluted it is to get the actual validation error messages though

## Backbone.js changes
- Created Dinners collection and Dinner model, pointed collection at the GET endpoint defined above and the model at the POST endpoint
- Created an App.Router to handle the two "pages" we're presenting to the user
- Created a Page.MainView definition to represent the two main controls on index.html, and handle switching between them
- Created a CreateDinnerView to handle the #create click event as well as serializing the input fields to a model and saving it - this is where validation will come in (error callback)
- Created a DinnersListView to call the collection's fetch event and render dinner templates once it returns
- Copied HTML from old project's razor views (index and create) and combined them into a single html page

## TODO
- Validation using messages from server
- More endpoints (get dinner by id, update existing dinner, delete dinner)
