■ 1
class Entity {
    constructor(public target: any) { }
    GetValue(propertyName: string) {
        var v = this[propertyName];
        return v != null ? v : this.target[propertyName];
    }
}

interface Point {
    x: number;
    y: number;
}
class EPoint extends Entity {
    constructor(target: Point) { super(target); }
    get norm() {
        return Math.sqrt(this.target.x * this.target.x + this.target.y * this.target.y);
    }
}

var p = <Point>{ "x": 2, "y": 3 };
var e = new Entity(p);
console.log(e.GetValue("x"));
var ep = new EPoint(p);
console.log(ep.GetValue("x"));
console.log(ep.norm);
console.log(ep.GetValue("norm"));

■ 2
function copy(src, dest) {
    if(src == null) return;
    if(dest == null) return;
    for(var p in src) {
        dest[p] = src[p];
    }
}
class Entity {
    constructor(obj) {
        copy(obj, this);
    }
}

class Point extends Entity {
    x: number;
    y: number;
    constructor(obj: Point) { super(obj); }
    get area(): number {
        return this.x * this.y;
    }
    get norm(): number {
        return Math.sqrt(this.x * this.x + this.y * this.y);
    }
}
class Class1 extends Entity {
    id: number;
    p2: Class2;
    constructor(obj: Class1) {
        super(obj); 
        if(obj.p2 != null) this.p2 = new Class2(obj.p2);
    }
    get message(): string {
        return this.id + "!";
    }
}
class Class2 extends Entity {
    id: number;
    p3: Class3;
    constructor(obj: Class2) {
        super(obj);
        if(obj.p3 != null) this.p3 = new Class3(obj.p3);
    }
    get message(): string {
        return this.id + "!";
    }
}
class Class3 extends Entity {
    id: number;
    constructor(obj: Class3) {
        super(obj);
    }
    get message(): string {
        return this.id + "!";
    }
}

var p = <Point>{ "x": 2, "y": 3 };
p = new Point(p);
console.log(p.x);
console.log(p.area);
console.log(p.norm);

var o = <Class1>{id:1,p2:{id:2,p3:{id:3}}};
o = new Class1(o);
console.log(o.message);
console.log(o.p2.id);
console.log(o.p2.message);
console.log(o.p2.p3.message);

■ 3 (JavaScript Only)
function defineProperty(o, name, getFunc, setFunc) {
    if(o == null) return;
    Object.defineProperty(o, name, {
        get: getFunc,
        set: setFunc,
        enumerable: true,
        configurable: true
    });
}
function extendPoint(o) {
    if(o == null) return;
    defineProperty(o, "area", function () {
        return this.x * this.y;
    });
    defineProperty(o, "norm", function () {
        return Math.sqrt(this.x * this.x + this.y * this.y);
    });
}
function extendClass1(o) {
    if(o == null) return;
    extendClass2(o.p2);
    defineProperty(o, "message", function () {
        return this.id + "!";
    });
}
function extendClass2(o) {
    if(o == null) return;
    o.p3.forEach(extendClass3);
    defineProperty(o, "message", function () {
        return this.id + "!";
    });
}
function extendClass3(o) {
    if(o == null) return;
    defineProperty(o, "message", function () {
        return this.id + "!";
    });
}

var p = { "x": 2, "y": 3 };
extendPoint(p);
console.log(p.x);
console.log(p.area);
console.log(p.norm);

var o = {id:1,p2:{id:2,p3:[{id:30},{id:31}]}};
extendClass1(o);
console.log(o.message);
console.log(o.p2.id);
console.log(o.p2.message);
console.log(o.p2.p3[0].message);

■ 4
function CapsuleProperty(src, dest, name) {
    dest["_" + name] = src[name];
    Object.defineProperty(dest, name, {
        get: function () {
            return this["_" + name];
        },
        set: function (v) {
            this["_" + name] = v;
            $(this).trigger("propertychanged", [name, v]);
        },
        enumerable: true,
        configurable: true
    });
}
class Entity {
    constructor(obj) {
        for(var p in obj) {
            CapsuleProperty(obj, this, p);
        }
    }
}

var p = { "x": 2, "y": 3 };
var ep = new Entity(p);
$(ep).on("propertychanged", function (e, n, v) {
    if (n != "x") return;
    console.log(this.x);
});
ep.x = 8;
ep.y = 9;
