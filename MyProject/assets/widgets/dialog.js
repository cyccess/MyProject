﻿/*!
 * 
 * 基于Bootstrap扩展Modal 
 * Date 2015-04-17 15:28
 * 
 */
+(function ($, window) {

    var dialog = function (config) {

        config = config || {};

        var setting = dialog.setting;

        // 合并默认配置
        for (var i in setting) {
            if (config[i] === undefined) config[i] = setting[i];
        }

        // 自定义按钮队列
        config.button = config.button || [];

        config.sure &&
        config.button.push({
            id: config.sureBtn,
            callback: config.sure
        });

        config.cancel &&
        config.button.push({
            id: config.cancelBtn,
            callback: config.cancel
        });

        return dialog.fn._init(config);
    }

    dialog.fn = dialog.prototype = {

        constructor: dialog,

        _init: function (config) {
            var that = this;

            that.config = config

            that._showBox();
            that.button.apply(that, config.button);
            that._addEvent();

            if (config.init) {
                //that.modal.on("shown.bs.modal", function (e) {
                //    config.init.call(that, window);
                //});

                setTimeout(function () {
                    config.init.call(that, window);
                }, 151);
            }

            return that;
        },
        _showBox: function () {
            var that = this,
                fn = that.config.close;

            that.modal = that._modal(that.config.template);
            that.modal.close = function () {
                if (typeof fn === 'function')
                    fn.call(that, window);
                this.removeFormDom();
            }

            return that;
        },
        button: function () {
            var that = this, buttons = [],
            listeners = that._listeners = that._listeners || {},
            ags = [].slice.call(arguments),
            item, value, id;

            for (var i in ags) {
                item = ags[i];
                id = item.id;

                if (!listeners[id])
                    listeners[id] = {};

                if (item.callback)
                    listeners[id].callback = item.callback;

            }
            return that;
        },
        _addEvent: function () {
            var that = this,
                modal = that.modal;

            modal.find(".modal-footer").on("click", function (event) {
                var target = event.target, btnID;
                if (target === modal.$close[0]) {
                    that._click('cancel');
                    return false;
                }
                else {
   
                    btnID = target.id;
                    btnID && that._click(target.id);
                }
            });

            return that;
        },
        _click: function (id) {
            var that = this,
                fn = that._listeners[id] && that._listeners[id].callback;
            return typeof fn !== "function" || fn.call(that, window) !== false ? that.modal.close() : that;
        },
        _modal: function (tem) {
            var that = this,
                modal = that.modal || $(".modal"),
                _close;

            modal.remove();
            modal = $(tem).modal("show");

            _close = modal.find('.close');
            if (_close) {
                modal["$close"] = _close;
            }

            modal.removeFormDom = function () {
                this.modal("hide");
            }

            return modal;
        }
    };

    dialog.fn._init.prototype = dialog.fn;

    /*! 使用jQ方式调用窗口 */
    $.fn.dialog = function () {
        var config = arguments;
        this.bind('click', function () { dialog.apply(this, config); return false; });
        return this;
    };

    window.dialog = $.dialog = dialog;


    /*! dialog 的全局默认配置 */
    dialog.setting = {
        template: null,         //对话框模版
        button: null,           //自定义按钮组
        sureBtn: "sure",        //确定按钮ID
        cancelBtn: "cancel",    //取消按钮ID
        sure: null,             //确定按钮回调函数
        cancel: null,           //取消按钮回调函数
        init: null,             //对话框初始化后执行的函数
        close: null             //对话框关闭前执行的函数
    };
}(jQuery, this));




/* 
 *使用示例
 *
 $.dialog({
    template: $("#select-user-dialog").html(),
    init: function () {
       
    },
    sure: function () {

        alert(1);
        return false;
    },
    cancel: function () {

    }
    button: [
        {
            id: "btn1",
            callback: function () {
                alert("btn1");
                return false;
            }
        }
    ],
    close: function () {
        alert("close");
    }
});
*/