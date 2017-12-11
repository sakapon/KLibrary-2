(function (KLibrary) {
    (function (ArrayEx) {
        Array.prototype.orderByBubble = function (toKey) {
            /// <summary>
            ///     要素をキーに従って昇順に並べ替えます。
            ///     バブル ソートを使用します。
            /// </summary>
            /// <param name="toKey" type="Function">キーを抽出する関数。this に配列、第 1 引数に要素、第 2 引数にインデックスが渡されます。</param>
            /// <returns type="Array" />

            if (toKey == null) return this;

            var keyed = this.select(function (x, i) { return { key: toKey.call(this, x, i), obj: x }; });
            return bubbleSort(keyed).select(function (x) { return x.obj; });
        };

        function bubbleSort(target) {
            /// <summary>
            ///     指定された配列自身をバブル ソートで並べ替えます。
            /// </summary>
            /// <param name="target" type="Array">並べ替える配列。</param>
            /// <returns type="Array" />

            for (var i = 0; i < target.length; i++) {
                for (var j = target.length - 1; i < j; j--) {
                    if (target[j - 1].key > target[j].key) {
                        var o = target[j - 1];
                        target[j - 1] = target[j];
                        target[j] = o;
                    }
                }
            }
            return target;
        }
    })(KLibrary.ArrayEx || (KLibrary.ArrayEx = {}));
    var ArrayEx = KLibrary.ArrayEx;
})(KLibrary || (KLibrary = {}));
