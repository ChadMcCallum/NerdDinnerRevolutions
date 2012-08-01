$().ready(function () {
    window.mainView = new Page.MainView();
});

App = {};

App.Dinner = Backbone.Model.extend({
    url: '/services/dinner'
});

App.Dinners = Backbone.Collection.extend({
   model: App.Dinner,
   url: '/services/dinners'
});

Page = {};

Page.MainView = Backbone.View.extend({
    el: $('body'),
    initialize: function () {
        this.dinnerCollection = new App.Dinners();
        this.dinnersView = new Page.DinnersView({ dinnerCollection: this.dinnerCollection });
        this.createDinnerView = new Page.CreateDinnerView();
        this.router = new App.Router();
        this.bindToRouter();
    },
    bindToRouter: function () {
        this.router.on("route:create", this.showCreateView, this);
        this.router.on("route:main", this.showListView, this);
        Backbone.history.start();
    },
    showCreateView: function () {
        this.dinnersView.$el.hide();
        this.createDinnerView.$el.show();
    },
    showListView: function () {
        this.createDinnerView.$el.hide();
        this.dinnersView.$el.show();
    }
});

Page.DinnersView = Backbone.View.extend({
    el: $('#dinner-list'),
    initialize: function (options) {
        this.dinnersListView = new Page.DinnersListView({ dinnerCollection: options.dinnerCollection });
    }
});

Page.CreateDinnerView = Backbone.View.extend({
    el: $('#create-dinner'),
    events: {
        'click #create': 'create'
    },
    create: function () {
        var model = new App.Dinner();
        model.save({
            Title: this.$('#Title').val(),
            EventDate: this.$('#EventDate').val(),
            Address: this.$('#Address').val(),
            HostedBy: this.$('#HostedBy').val()
        }, {
            success: function () {
                //need a nicer way of navigating to this
                window.mainView.router.navigate('', { trigger: true });
            },
            error: function () {
                //validation goes here :P
            }
        });

    }
});

Page.DinnersListView = Backbone.View.extend({
    el: $('#list'),
    initialize: function (options) {
        if (options.dinnerCollection != null) {
            this.bindDinnerCollection(options.dinnerCollection);
        }
    },
    bindDinnerCollection: function (dinnerCollection) {
        this.dinnerCollection = dinnerCollection;
        this.dinnerCollection.bind('reset', this.render, this);
        this.dinnerCollection.fetch();
    },
    render: function () {
        var html = "";
        var template = _.template("<li><%= Title %> (<%= EventDate %>)</li>");
        this.dinnerCollection.each(function (dinner) {
            html += template(dinner.toJSON());
        });
        this.$el.html(html);
    }
});

App.Router = Backbone.Router.extend({
    routes: {
        "home/create": "create",
        "": "main"
    }
})