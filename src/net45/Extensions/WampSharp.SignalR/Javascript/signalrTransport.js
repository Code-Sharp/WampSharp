function Factory(options) {
    var self = this;

    autobahn.util.assert(options.url !== undefined, "options.url missing");
    autobahn.util.assert(typeof options.url === "string", "options.url must be a string");

    self._options = options;
}


Factory.prototype.type = "signalR";


Factory.prototype.create = function () {

    var self = this;

    // the WAMP transport we create
    var transport = {};

    // these will get defined further below
    transport.protocol = undefined;
    transport.send = undefined;
    transport.close = undefined;

    // these will get overridden by the WAMP session using this transport
    transport.onmessage = function () { };
    transport.onopen = function () { };
    transport.onclose = function () { };

    transport.info = {
        type: 'signalR',
        url: null,
        protocol: 'wamp.2.json'
    };

    //
    // running in browser
    //
    //if ('window' in global) {
    if (true) {

        (function () {

            var connection = $.connection(self._options.url);

            connection.received(function (data) {
                autobahn.log.debug("SignalR transport receive", data);
                var msg = JSON.parse(data);
                transport.onmessage(msg);
            });

            var start = function () {
                transport.info.url = self._options.url;
                transport.onopen();
            };

            connection.error(function (error) {
                var details = {
                    error: error
                };

                transport.onclose(details);
            });

            connection.start({
                transport: ['longPolling']
                //transport: ['webSockets']
                //jsonp : true
            }, start);
            //connection.start({ jsonp: true }, start);

            transport.send = function (msg) {
                var payload = JSON.stringify(msg);
                autobahn.log.debug("SignalR transport send", payload);
                connection.send(payload);
            };

            transport.close = function (code, reason) {
                connection.stop();
            };

        })();
    }

    return transport;
};