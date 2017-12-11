interface ServiceResponse {
    MessageType: number;
    Message: string;
    Result: any;
}

class MessageType {
    static None: number = 0;
    static Information: number = 1;
    static Warning: number = 2;
    static Error: number = 3;
}

// 例:
//interface Entity1 {
//	Prop1: string;
//}
//var objectFromJson = {"MessageType":0,"Message":null,"Result":{"Prop1":"Value1"}};
//
//var response = <ServiceResponse>objectFromJson;
//var result = <Entity1>response.Result;
//alert(result.Prop1);
