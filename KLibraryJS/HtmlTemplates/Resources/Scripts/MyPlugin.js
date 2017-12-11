(function ($) {
    $.myPlugin = {
        // 既定の設定値です。
        settings: {
            a: 1,
            b: "2"
        }
    };

    $.fn.myPlugin = function (options) {
        /// <summary>
        ///     プラグインの説明です。
        /// </summary>
        /// <param name="options" type="Object">オプション。</param>
        /// <returns type="jQuery" />

        return this.each(function () {
            // 設定値をマージします。
            // data() で取得される値の型には注意が必要です。
            var settings = $.extend({}, $.myPlugin.settings, options, $(this).data());

            // 各要素に対して処理を実行します。
            doWork($(this), settings);
        });
    };

    function doWork($element, settings) {
        // ここに、各要素に対する処理を記述します。
    }
})(jQuery);
