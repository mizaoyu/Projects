var express = require('express');
var http = require('http');
var app = express();

// The number of milliseconds in one day
var oneDayMS = 24 * 60 * 60 * 1000;

// Use compress middleware to gzip content
app.use(express.compress());

// rest APIs
app.use(express.bodyParser());

// fake product list
var productItems = [
    { Id: 1, Name: "Woman's T - Shirt A", Picture: "images/p1.jpg", Price: 129, Discount: 0 },
    { Id: 2, Name: "Woman's T - Shirt B", Picture: "images/p2.jpg", Price: 89, Discount: 0 },
    { Id: 3, Name: "Woman's T - Shirt C", Picture: "images/p3.jpg", Price: 149, Discount: 0 },
    { Id: 4, Name: "Woman's T - Shirt D", Picture: "images/p4.jpg", Price: 40, Discount: 8 },
    { Id: 5, Name: "Woman's T - Shirt E", Picture: "images/p5.jpg", Price: 86, Discount: 17.2 },
    { Id: 6, Name: "Woman's T - Shirt F", Picture: "images/p6.jpg", Price: 149, Discount: 29.8 },
    { Id: 7, Name: "Beautiful Shoes", Picture: "images/p7.jpg", Price: 36, Discount: 7.2 },
    { Id: 8, Name: "Leather Boots", Picture: "images/p8.jpg", Price: 86, Discount: 17.2 },
    { Id: 9, Name: "Canvas Shoes", Picture: "images/p9.jpg", Price: 60, Discount: 12 },
    { Id: 10, Name: "Skateboard Shoes", Picture: "images/p10.jpg", Price: 129, Discount: 25.8 },
];

var cartItems = [];

// Items controller
app.get("/api/items/get", function items$list(req, res) {
    res.json(productItems);
});

app.get("/api/items/get/:id", function items$get(req, res) {
    if (productItems.length <= req.params.id) {
        res.statusCode = 404;
        return res.send('Error 404: No product item found');
    }

    res.json(productItems[req.params.id]);
});

// Cart
app.get("/api/cart/get", function cart$list(req, res) {
    res.json(cartItems);
});

app.get("/api/cart/getcount", function cart$get(req, res) {
    res.json(cartItems.length);
});

app.post("/api/cart/post", function cart$post(req, res) {
    if (!req.body.hasOwnProperty('ItemId') ||
       !req.body.hasOwnProperty('Color') ||
       !req.body.hasOwnProperty('Quantity') ||
       !req.body.hasOwnProperty('Size') ||
       !productItems[req.body.ItemId]) {
        res.statusCode = 400;
        return res.send('Error 400: Post syntax incorrect.');
    }

    var item = productItems[req.body.ItemId];
    var cartItem;

    for (var i in cartItems)
    {
        if (cartItems[i].ItemId === req.body.ItemId &&
            cartItems[i].Color === req.body.Color &&
            cartItems[i].Size === req.body.Size)
        {
            cartItem = cartItems[i];
            break;
        }
    }

    if (!!cartItem) {
        cartItem.Quantity += req.body.Quantity;
        cartItem.TotalSize = (cartItem.Price - cartItem.Discount) * cartItem.Quantity;
    } else {
        cartItems.push({
            Name: item.Name,
            Picture: item.Picture,
            Price: item.Price,
            Discount: item.Discount,
            TotalPrice: (item.Price - item.Discount) * req.body.Quantity,
            Color: req.body.Color,
            ItemId: req.body.ItemId,
            Quantity: req.body.Quantity,
            Size: req.body.Size
        });
    }
    return res.status(200).end();
});

app.post("/api/payment/post", function payment$creat(req, res, next) {
    var postData = JSON.stringify(req.body);
    var options = {
        hostname: 'shacsd3600',
        port: 9001,
        path: '/api/v1/payments/create',
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Content-Length': postData.length
        }
    };

    var reqClient = http.request(options, function (resPayment) {
        resPayment.on('error', function (e) {
            console.log('ERROR: ' + e);
            res.status(500).send(e.message);
        });
        resPayment.on('data', function (chunk) {
            res.json(JSON.parse(chunk)).end();
        });
        //next();
    });
    reqClient.write(postData);
    reqClient.end();
});

// Serve up content from current directory
app.use(express.directory(__dirname));
app.use(express.static(__dirname, { maxAge: oneDayMS }));

app.listen(process.env.MerchantSitePORT || 9000);