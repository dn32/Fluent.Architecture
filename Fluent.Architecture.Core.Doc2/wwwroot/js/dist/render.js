var Render = (function () {
    var defines = {};
    var entry = [null];
    function define(name, dependencies, factory) {
        defines[name] = { dependencies: dependencies, factory: factory };
        entry[0] = name;
    }
    define("require", ["exports"], function (exports) {
        Object.defineProperty(exports, "__cjsModule", { value: true });
        Object.defineProperty(exports, "default", { value: function (name) { return resolve(name); } });
    });
    var __extends = (this && this.__extends) || (function () {
        var extendStatics = function (d, b) {
            extendStatics = Object.setPrototypeOf ||
                ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
                function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
            return extendStatics(d, b);
        };
        return function (d, b) {
            extendStatics(d, b);
            function __() { this.constructor = d; }
            d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
        };
    })();
    define("Model/FluentReferenceAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentAggregationAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentPagination", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/DefaultResult", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/DefaultPaginationResult", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/KeyValues", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var KeyValues = /** @class */ (function () {
            function KeyValues(key, value) {
                this.Key = key;
                this.Values = [];
                this.addValue(value);
            }
            KeyValues.prototype.addValue = function (value) {
                this.Values.push(value);
            };
            return KeyValues;
        }());
        exports.KeyValues = KeyValues;
    });
    define("Enum/EnumForm", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var EnumForm;
        (function (EnumForm) {
            // NONE = 0,
            EnumForm[EnumForm["TEXTBOX"] = 1] = "TEXTBOX";
            EnumForm[EnumForm["COMBOBOX"] = 2] = "COMBOBOX";
            EnumForm[EnumForm["NUMBER"] = 4] = "NUMBER";
            EnumForm[EnumForm["DATEPICKER"] = 5] = "DATEPICKER";
            EnumForm[EnumForm["FILE"] = 6] = "FILE";
            // MODAL = 6,
            EnumForm[EnumForm["FORM"] = 7] = "FORM";
            EnumForm[EnumForm["HIDDEN"] = 8] = "HIDDEN";
        })(EnumForm = exports.EnumForm || (exports.EnumForm = {}));
    });
    define("Enum/EnumFilterType", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var EnumFilterType;
        (function (EnumFilterType) {
            EnumFilterType[EnumFilterType["GREATER"] = 0] = "GREATER";
            EnumFilterType[EnumFilterType["SMALLER"] = 1] = "SMALLER";
            EnumFilterType[EnumFilterType["START_WITH"] = 2] = "START_WITH";
            EnumFilterType[EnumFilterType["ENDS_WITH"] = 3] = "ENDS_WITH";
            EnumFilterType[EnumFilterType["CONTAINS"] = 4] = "CONTAINS";
            EnumFilterType[EnumFilterType["EQUAL"] = 5] = "EQUAL";
            EnumFilterType[EnumFilterType["NULL"] = 6] = "NULL";
            EnumFilterType[EnumFilterType["TRUE"] = 7] = "TRUE";
            EnumFilterType[EnumFilterType["FALSE"] = 8] = "FALSE";
        })(EnumFilterType = exports.EnumFilterType || (exports.EnumFilterType = {}));
    });
    define("Enum/EnumJunctionType", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var EnumJunctionType;
        (function (EnumJunctionType) {
            EnumJunctionType[EnumJunctionType["AND"] = 0] = "AND";
            EnumJunctionType[EnumJunctionType["OR"] = 1] = "OR";
        })(EnumJunctionType = exports.EnumJunctionType || (exports.EnumJunctionType = {}));
    });
    define("Enum/Filter", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var Filter = /** @class */ (function () {
            function Filter() {
            }
            return Filter;
        }());
        exports.Filter = Filter;
    });
    define("Model/FluentJsoSchemaAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentJsonFormAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentJsonSchema", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentCompositionAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/KeyValue", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var KeyValue = /** @class */ (function () {
            function KeyValue() {
            }
            return KeyValue;
        }());
        exports.KeyValue = KeyValue;
        var Dictionary = /** @class */ (function (_super) {
            __extends(Dictionary, _super);
            function Dictionary() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return Dictionary;
        }(KeyValue));
        exports.Dictionary = Dictionary;
    });
    define("Model/EnumGrupType", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var EnumGrupType;
        (function (EnumGrupType) {
            EnumGrupType[EnumGrupType["REGION"] = 0] = "REGION";
            EnumGrupType[EnumGrupType["TAB"] = 1] = "TAB";
            EnumGrupType[EnumGrupType["WIZARD"] = 2] = "WIZARD";
        })(EnumGrupType = exports.EnumGrupType || (exports.EnumGrupType = {}));
    });
    define("Model/FluentJsonPropertyAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentError", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentInconsistence", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentPropertyInconsistence", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/FluentUiFieldInconsistence", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/ValidationReturn", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Tests/FluentNode", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Util/Globalization", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        function G(key, plural) {
            if (plural === void 0) { plural = false; }
            if (key == null || key == '') {
                return key;
            }
            var firstChar = key[0];
            var FistCharIsUpercase = false;
            FistCharIsUpercase = firstChar == firstChar.toUpperCase();
            key = key.toLowerCase().trim();
            var keyValue = dictionary.filter(function (x) { return x.Key.toLowerCase() == key && x.Plural == plural; })[0];
            var value = key;
            if (keyValue != null) {
                value = keyValue.Value.toLowerCase();
            }
            if (FistCharIsUpercase) {
                value = value[0].toUpperCase() + value.substr(1).toLowerCase();
            }
            return value;
        }
        exports.G = G;
        var dictionary = [
            { Key: "page", Value: "Página", Plural: false },
            { Key: "Items per page", Value: "Itens por página", Plural: true },
            { Key: "search", Value: "buscar", Plural: false },
            { Key: "found", Value: "encontrados", Plural: true },
            { Key: "Enter a value for the value field", Value: "Informe um valor", Plural: false },
            { Key: "Click to maximize", Value: "Clique para maximizar", Plural: false },
            { Key: "Click to minimize", Value: "Clique para minimizar", Plural: false },
            { Key: "Filters", Value: "Filtros", Plural: false },
            { Key: "Filter type", Value: "Tipo de filtro", Plural: false },
            { Key: "Revert", Value: "Reverter", Plural: false },
            { Key: "added", Value: "adicionado", Plural: false },
            { Key: "added", Value: "adicionados", Plural: true },
            { Key: "Value", Value: "Valor", Plural: false },
            { Key: "not", Value: "não", Plural: false },
            { Key: "Field", Value: "Campo", Plural: false },
            { Key: "Update", Value: "atualizar", Plural: false },
            { Key: "from", Value: "de", Plural: false },
            { Key: "to", Value: "a", Plural: false },
            { Key: "View", Value: "Visualizar", Plural: false },
            { Key: "Remove", Value: "Remover", Plural: false },
            { Key: "for", Value: "para", Plural: false },
            { Key: "Add", Value: "Adicionar", Plural: false },
            { Key: "Select", Value: "Selecione", Plural: false },
            { Key: "submit", Value: "Salvar", Plural: false },
            { Key: "What do you want to manipulate?", Value: "O que você quer manipular?", Plural: false },
            { Key: "What operation do you want to perform?", Value: "Que operação você deseja executar?", Plural: false },
            { Key: "and", Value: "e", Plural: false },
            { Key: "or", Value: "ou", Plural: false },
            { Key: "equal", Value: "Igual", Plural: false },
            { Key: "Contains", Value: "Contém", Plural: false },
            { Key: "ends_with", Value: "Termina com", Plural: false },
            { Key: "start_with", Value: "Inicia com", Plural: false },
            { Key: "null", Value: "nulo", Plural: false },
            { Key: "associates", Value: "associados", Plural: true },
            { Key: "available", Value: "disponíveis", Plural: true },
            { Key: 'Saved Successfully', Value: "Salvo com sucesso", Plural: false },
            { Key: 'Sucess!', Value: "Sucesso!", Plural: false },
            { Key: 'Error!', Value: "Erro!", Plural: false },
            { Key: 'Remove', Value: "Remover", Plural: false },
            { Key: 'Clone', Value: "Duplicar", Plural: false },
            { Key: 'Removed Successfully', Value: "Removido com sucesso", Plural: false },
            { Key: 'New', Value: "Novo", Plural: false },
            { Key: 'Server connection fail', Value: "Falha na conexão com servidor", Plural: false },
            { Key: 'Finding...', Value: "Buscando...", Plural: false },
            { Key: 'An item of type {0} that associates with this form is no longer present in the system.', Value: "Um item do tipo {0} que se associa a esse formulário não está mais presente no sistema", Plural: false },
            { Key: 'Element not found with name {0}', Value: "Um componente de tela está ausente, seu nome é {0}", Plural: false },
            { Key: 'Element not found with id {0}', Value: "Um componente de tela está ausente, seu id é {0}", Plural: false },
            { Key: 'No results', Value: "Nenhum item encontrado", Plural: false },
            { Key: 'The request for type {0} resulted in more than one value. There is a duplicate in the system and we were unable to determine which item would suit your request.', Value: 'A solicitação para o tipo {0} resultou em mais de um valor. Há uma duplicidade no sistema e por isso não foi possível determinar qual item seria adequado à sua solicitação.', Plural: false },
            { Key: 'Loadin data...', Value: 'Obtendo dados...', Plural: false },
        ];
    });
    define("Services/MessageServiceClass", ["require", "exports", "Util/Util", "Util/Globalization"], function (require, exports, UTIL, Globalization_1) {
        "use strict";
        exports.__esModule = true;
        var MessageServiceClass = /** @class */ (function () {
            function MessageServiceClass(containerForm) {
                this.ContainerForm = containerForm;
                this.InitializeComponents();
                this.AttachEvents();
            }
            MessageServiceClass.prototype.InitializeComponents = function () {
                this.MessageContainer = UTIL.GetElByNameOnAnotherElement(null, this.ContainerForm, 'message-container');
                this.MessageSucessComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageContainer, 'message-sucess');
                this.MessageErrorComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageContainer, 'message-error');
                // this.MessageWharningComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageContainer, 'message-wharning') as HTMLElement;
                // this.MessageInfoComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageContainer, 'message-info') as HTMLElement;
                this.MessageSucessCloseComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageSucessComponent, 'btn-close-message');
                this.MessageErrorCloseComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageErrorComponent, 'btn-close-message');
                // this.MessageWharningCloseComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageWharningComponent, 'btn-close-message') as HTMLElement;
                // this.MessageInfoCloseComponent = UTIL.GetElByNameOnAnotherElement(null, this.MessageInfoComponent, 'btn-close-message') as HTMLElement;
            };
            MessageServiceClass.prototype.AttachEvents = function () {
                var _this = this;
                this.MessageSucessCloseComponent.addEventListener('click', function (e) {
                    _this.Clear();
                });
                this.MessageErrorCloseComponent.addEventListener('click', function (e) {
                    _this.Clear();
                });
                // this.MessageWharningCloseComponent.addEventListener('click', (e) => {
                //     this.Clear();
                // });
                // this.MessageInfoCloseComponent.addEventListener('click', (e) => {
                //     this.Clear();
                // });
            };
            MessageServiceClass.prototype.Clear = function () {
                this.MessageSucessComponent.hidden = true;
                this.MessageErrorComponent.hidden = true;
                // this.MessageWharningComponent.hidden = true;
                // this.MessageInfoComponent.hidden = true;
            };
            // Info(message: string) {
            //     this.Message(this.MessageInfoComponent, G('Info!'), message);
            // }
            // Wharning(message: string) {
            //     this.Message(this.MessageWharningComponent, G('Wharning!'), message);
            // }
            MessageServiceClass.prototype.Error = function (message) {
                if (UTIL.IsJsonString(message)) {
                    var error = JSON.parse(message);
                    if (error.Message != null) {
                        message = error.Message;
                    }
                }
                this.Message(this.MessageErrorComponent, Globalization_1.G('Error!'), message);
            };
            MessageServiceClass.prototype.Sucess = function (message) {
                this.Message(this.MessageSucessComponent, Globalization_1.G('Sucess!'), message);
            };
            MessageServiceClass.prototype.Message = function (component, title, message) {
                this.Clear();
                component.hidden = false;
                component.querySelector('strong').textContent = title;
                component.querySelector('span').textContent = message;
            };
            return MessageServiceClass;
        }());
        exports.MessageServiceClass = MessageServiceClass;
    });
    define("Model/RenderPamaterers", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var RenderPamaterers = /** @class */ (function () {
            function RenderPamaterers() {
            }
            return RenderPamaterers;
        }());
        exports.RenderPamaterers = RenderPamaterers;
    });
    define("IRenderClass", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Model/CONSTANT", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var CONSTANT = /** @class */ (function () {
            function CONSTANT() {
            }
            CONSTANT.SetParameters = function (parameters) {
                CONSTANT.BASE_CLIENT_URL = parameters.ClientUrl;
                CONSTANT.BASE_API_URL = parameters.ApiUrl;
                CONSTANT.BASE_FORMS_URL = parameters.FormsUrl;
                CONSTANT.USE_CACHE = parameters.UseCache;
                CONSTANT.PROXIMITY_FIND_TOLERANCE = parameters.ProximityFindTolerance;
                CONSTANT.URL_CSS_BOOTSTRAP = CONSTANT.BASE_FORMS_URL + "?file=/css/lib/bootstrap.min.css";
                CONSTANT.URL_CSS_SELECT2 = CONSTANT.BASE_FORMS_URL + "?file=/css/lib/select2.min.css";
                CONSTANT.URL_CSS_SELECT2_BOOTSTRAP = CONSTANT.BASE_FORMS_URL + "?file=/css/lib/select2-bootstrap.min.css";
                CONSTANT.URL_COMPONENTS_CSS = CONSTANT.BASE_FORMS_URL + "?file=/css/components.css";
                CONSTANT.URL_MAIN_CSS = CONSTANT.BASE_FORMS_URL + "?file=/css/main.css";
                CONSTANT.URL_JS_JQUERY = CONSTANT.BASE_FORMS_URL + "?file=/js/lib/jquery-3.4.1.min.js";
                CONSTANT.URL_JS_SELECT2 = CONSTANT.BASE_FORMS_URL + "?file=/js/lib/select2.min.js";
                CONSTANT.URL_JS_POPPER = CONSTANT.BASE_FORMS_URL + "?file=/js/lib/popper.min.js";
                CONSTANT.URL_JS_BOOTSTRAP = CONSTANT.BASE_FORMS_URL + "?file=/js/lib/bootstrap.min.js";
                CONSTANT.URL_TEMPLATE = CONSTANT.BASE_FORMS_URL + "?file=/templates/index.txt";
                CONSTANT.URL_IMG = CONSTANT.BASE_FORMS_URL + "?file=/image";
                CONSTANT.URL_IMG_ADD = CONSTANT.URL_IMG + "/add.png";
                CONSTANT.URL_IMG_REMOVE = CONSTANT.URL_IMG + "/remove.png";
                CONSTANT.URL_IMG_WAIT = CONSTANT.URL_IMG + "/wait.gif";
                CONSTANT.URL_ANGULAR = CONSTANT.BASE_CLIENT_URL + "?file=/html/index.html&form={0}";
                CONSTANT.URL_JSONFORM = CONSTANT.BASE_API_URL + "/api/{0}/JsonForm?tablet={1}";
                CONSTANT.URL_VERSION = CONSTANT.BASE_API_URL + "/api/version";
                CONSTANT.URL_DATA_LIST_BY_FILTER = CONSTANT.BASE_API_URL + "/api/{0}/ListByFilter";
                CONSTANT.URL_DATA_FIND_BY_FILTER = CONSTANT.BASE_API_URL + "/api/{0}/FindByFilter";
                CONSTANT.URL_TRUNCATE = CONSTANT.BASE_API_URL + "/api/{0}/Truncate";
                CONSTANT.URL_DATA_FIND_BY_TERM = CONSTANT.BASE_API_URL + "/api/{0}/FindByTerm?term={1}";
                CONSTANT.URL_DATA_ADD = CONSTANT.BASE_API_URL + "/api/{0}/Add";
                CONSTANT.URL_DATA_UPDATE = CONSTANT.BASE_API_URL + "/api/{0}/Update";
                CONSTANT.URL_DATA_ADD_OR_UPDATE = CONSTANT.BASE_API_URL + "/api/{0}/AddOrUpdate";
                CONSTANT.URL_DATA_REMOVE = CONSTANT.BASE_API_URL + "/api/{0}/Remove";
                CONSTANT.URL_DATA_FOR_TEST = CONSTANT.BASE_API_URL + "/api/TestSupport/GetAllInstances";
                // CONSTANT.URL_DATA_FIND_BY_PROXIMITY = `${CONSTANT.BASE_API_URL}/api/{0}/FindByProximity`;
                CONSTANT.URL_DATA_FIND_BY_PROXIMITY = CONSTANT.BASE_API_URL + "/api/{0}/ListByTerm";
                CONSTANT.NEXT_ARRAY_VALUE = 0;
            };
            CONSTANT.NextArrayValue = function () {
                return CONSTANT.NEXT_ARRAY_VALUE++;
            };
            CONSTANT.SCRIPT_CACHE_CANCELATION = Math.random().toString(10).slice(2);
            return CONSTANT;
        }());
        exports.CONSTANT = CONSTANT;
    });
    define("Cache/Cache", ["require", "exports", "Model/CONSTANT"], function (require, exports, CONSTANT_1) {
        "use strict";
        exports.__esModule = true;
        function Set(key, value) {
            if (!CONSTANT_1.CONSTANT.USE_CACHE) {
                return;
            }
            var json = JSON.stringify(value);
            localStorage.setItem(key, json);
        }
        exports.Set = Set;
        function Get(key) {
            if (!CONSTANT_1.CONSTANT.USE_CACHE) {
                return null;
            }
            var json = localStorage.getItem(key);
            return JSON.parse(json);
        }
        exports.Get = Get;
        function Exists(key) {
            if (!CONSTANT_1.CONSTANT.USE_CACHE) {
                return false;
            }
            return localStorage.getItem(key) != null;
        }
        exports.Exists = Exists;
        function Remove(key) {
            localStorage.removeItem(key);
        }
        exports.Remove = Remove;
        function Clear() {
            localStorage.clear();
        }
        exports.Clear = Clear;
    });
    define("API/API", ["require", "exports", "Util/Util", "Util/Util", "Cache/Cache", "Model/CONSTANT"], function (require, exports, Util_1, UTIL, CACHE, CONSTANT_2) {
        "use strict";
        exports.__esModule = true;
        function AddOrUpdate(render, type, json, callBack) {
            var Http = new XMLHttpRequest();
            var url = '';
            if (render.IsEditOrNew) {
                url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_DATA_ADD_OR_UPDATE, type);
                Http.open('POST', url);
            }
            else if (render.IsEdit) {
                url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_DATA_UPDATE, type);
                Http.open('PUT', url);
            }
            else {
                url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_DATA_ADD, type);
                Http.open('POST', url);
            }
            Http.setRequestHeader("Content-Type", "application/json");
            Http.send(json);
            Http.onreadystatechange = function (e) {
                if (Http.readyState == 4) {
                    callBack(render, Http.status, Http.responseText);
                }
            };
        }
        exports.AddOrUpdate = AddOrUpdate;
        function GetJsonSchema(render, type, callback) {
            var _this = this;
            var url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_JSONFORM, type, "false");
            var schema = CACHE.Get(url);
            if (schema != null) {
                callback.call(_this, schema);
                return;
            }
            GetDataFromServer.call(_this, render, url, function (data) {
                CACHE.Set(url, data);
                callback.call(_this, data);
            });
        }
        exports.GetJsonSchema = GetJsonSchema;
        function ResponseIs200(render, Http) {
            if (Http.readyState == 4) {
                if (Http.status == 200) {
                    if (UTIL.IsJsonString(Http.responseText)) {
                        return JSON.parse(Http.responseText);
                    }
                    else {
                        return Http.responseText;
                    }
                }
                else {
                    UTIL.Exception(render, Http.responseText, Http.status);
                    return null;
                }
            }
            else {
                return null;
            }
        }
        exports.ResponseIs200 = ResponseIs200;
        function ListFilter(render, type, jsonFilter, header, callback) {
            var Http = new XMLHttpRequest();
            var url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_DATA_LIST_BY_FILTER, type);
            Http.open('POST', url);
            Http.setRequestHeader("Content-Type", "application/json");
            if (header != null) {
                header.forEach(function (element) {
                    Http.setRequestHeader(element.Key, element.Value);
                });
            }
            Http.send(jsonFilter);
            var _this = this;
            Http.onreadystatechange = function (e) {
                var result = ResponseIs200(render, Http);
                if (result != null) {
                    if (UTIL.IsJsonString(Http.responseText)) {
                        callback.call(_this, JSON.parse(Http.responseText));
                    }
                }
            };
        }
        exports.ListFilter = ListFilter;
        function FindByFilter(render, type, jsonFilter, header, callback) {
            var Http = new XMLHttpRequest();
            var url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_DATA_FIND_BY_FILTER, type);
            Http.open('POST', url);
            Http.setRequestHeader("Content-Type", "application/json");
            if (header != null) {
                header.forEach(function (element) {
                    Http.setRequestHeader(element.Key, element.Value);
                });
            }
            Http.send(jsonFilter);
            var _this = this;
            Http.onreadystatechange = function (e) {
                var result = ResponseIs200(render, Http);
                if (result != null) {
                    if (UTIL.IsJsonString(Http.responseText)) {
                        callback.call(_this, JSON.parse(Http.responseText));
                    }
                }
            };
        }
        exports.FindByFilter = FindByFilter;
        // export function FindOneByFilter(render: IRenderClass, type: string, jsonFilter: string, callback: (data: any) => void) {
        //     this.ListFilter(render, type, jsonFilter, null, (list: DefaultPaginationResult) => {
        //         if (list == null) { return null; }
        //         if (list.Pagination.TotalQuantityOfItems > 1) {
        //             UTIL.Exception(render, UTIL.FormatString(G('The request for type {0} resulted in more than one value. There is a duplicate in the system and we were unable to determine which item would suit your request.'), type));
        //             throw (`More than one item was found with the keys entered. Filter ${jsonFilter}`); return;
        //         }
        //         if (list.Pagination.TotalQuantityOfItems == 0) {
        //             UTIL.Exception(render, UTIL.FormatString(G(`An item of type {0} that associates with this form is no longer present in the system.`), type));
        //             throw (`Item not found. Filter ${jsonFilter}`);
        //         }
        //         callback(list.Data[0]);
        //     });
        //}
        function GetDataFromServer(render, url, callback) {
            var Http = new XMLHttpRequest();
            Http.open("GET", url);
            Http.send();
            Http.onreadystatechange = function (e) {
                var result = ResponseIs200(render, Http);
                if (result != null) {
                    if (UTIL.IsJsonString(Http.responseText)) {
                        callback(JSON.parse(Http.responseText));
                    }
                    else {
                        callback(Http.responseText);
                    }
                }
            };
        }
        exports.GetDataFromServer = GetDataFromServer;
        function Remove(render, type, json, callBack) {
            var Http = new XMLHttpRequest();
            var url = Util_1.FormatString(CONSTANT_2.CONSTANT.URL_DATA_REMOVE, type);
            Http.open('DELETE', url);
            Http.setRequestHeader("Content-Type", "application/json");
            Http.send(json);
            Http.onreadystatechange = function (e) {
                if (Http.readyState == 4) {
                    callBack(render, Http.status, Http.responseText);
                }
            };
        }
        exports.Remove = Remove;
        function Truncate(render, type, callback) {
            var Http = new XMLHttpRequest();
            var url = UTIL.FormatString(CONSTANT_2.CONSTANT.URL_TRUNCATE, type);
            Http.open("DELETE", url);
            Http.setRequestHeader('ERASE_ALL_DATA', 'yes');
            Http.send();
            Http.onreadystatechange = function (e) {
                var result = ResponseIs200(render, Http);
                if (result != null) {
                    if (UTIL.IsJsonString(Http.responseText)) {
                        callback(JSON.parse(Http.responseText));
                    }
                    else {
                        callback(Http.responseText);
                    }
                }
            };
        }
        exports.Truncate = Truncate;
    });
    define("Util/Util", ["require", "exports", "Model/KeyValues", "Enum/EnumForm", "Enum/EnumJunctionType", "Enum/EnumFilterType", "Util/Globalization", "Model/CONSTANT"], function (require, exports, KeyValues_1, EnumForm_1, EnumJunctionType_1, EnumFilterType_1, Globalization_2, CONSTANT_3) {
        "use strict";
        exports.__esModule = true;
        function FormatString(str) {
            var val = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                val[_i - 1] = arguments[_i];
            }
            for (var index = 0; index < val.length; index++) {
                str = str.replace("{" + index + "}", val[index]);
            }
            return str;
        }
        exports.FormatString = FormatString;
        function ReplaceAll(text, oldString, newString) {
            if (text.length == 0) {
                return text;
            }
            if (text.length == 1) {
                return text.replace(oldString, newString);
            }
            return text.split(oldString).join(newString);
        }
        exports.ReplaceAll = ReplaceAll;
        function FieldsToObject(parentHtmlElement) {
            var elements = parentHtmlElement.querySelectorAll("input, select, textarea");
            elements.forEach(function (el) {
                var element = el;
                var name = element.name;
                var value = element.value;
                if (name.indexOf('[')) {
                    //Array
                }
                if (name.indexOf('.')) {
                    //complex object
                }
            });
        }
        exports.FieldsToObject = FieldsToObject;
        function getObject(data, name, value) {
            var firstArrayStartPosiction = name.indexOf('[');
            var firstArrayEndPosiction = name.indexOf(']');
            var firstObjectStartPosiction = name.indexOf('.');
            var firstObjecEndtPosiction = name.indexOf('.', firstObjectStartPosiction);
            if (firstArrayStartPosiction < firstObjectStartPosiction || firstObjectStartPosiction == -1) {
                if (firstArrayStartPosiction == -1) {
                    data[name] = value;
                    return;
                }
                var firstName = name.substring(0, firstArrayStartPosiction);
                var arrayKey = name.substring(firstArrayStartPosiction + 1, firstArrayEndPosiction);
                var arrayKeyNumber = Number(arrayKey);
                var lastName = name.substring(firstArrayEndPosiction + 2);
                if (lastName == '') {
                    if (data == null) {
                        data = [];
                    }
                    for (var i = 0; i <= arrayKeyNumber; i++) {
                        if (data[i] == null) {
                            data[i] = {};
                        }
                    }
                    data[arrayKeyNumber] = value;
                }
                else {
                    if (data[firstName] == null) {
                        data[firstName] = [];
                    }
                    for (var i = 0; i <= arrayKeyNumber; i++) {
                        if (data[firstName][i] == null) {
                            data[firstName][i] = {};
                        }
                    }
                    getObject(data[firstName][arrayKeyNumber], lastName, value);
                }
                return; //array
            }
            else if (firstObjectStartPosiction >= 0) {
                var firstName = name.substring(0, firstObjectStartPosiction);
                var min = Math.min(firstObjecEndtPosiction, firstArrayStartPosiction);
                var lastName = name.substring(min + 1);
                if (data[firstName] == null) {
                    data[firstName] = {};
                }
                ;
                getObject(data[firstName], lastName, value);
                return;
            }
            data[name] = value;
        }
        exports.getObject = getObject;
        function removeNulls(obj) {
            var isArray = obj instanceof Array;
            for (var k in obj) {
                if (obj[k] === null || Object.keys(obj[k]).length == 0) {
                    isArray ? obj.splice(k, 1) : delete obj[k];
                }
                else if (typeof obj[k] == "object") {
                    removeNulls(obj[k]);
                }
                if (isArray && obj.length == k) {
                    removeNulls(obj);
                }
            }
            return obj;
        }
        exports.removeNulls = removeNulls;
        function FormToJSONString(render, parentHtmlElement, ignore) {
            if (ignore === void 0) { ignore = null; }
            var elements = parentHtmlElement.querySelectorAll("input, select, textarea");
            var data = {};
            parentHtmlElement.querySelectorAll('.form-group').forEach(function (element) {
                element.classList.remove('is-invalid-field');
            });
            ;
            var _loop_1 = function (i) {
                var element = elements[i];
                if (element.classList.contains('select2')) {
                    return "continue";
                }
                var name_1 = element.name;
                var value = element.value;
                if (element.type != 'hidden') {
                    var elementError = GetElByNameOnAnotherElement(render, parentHtmlElement, "error-" + name_1);
                    if (elementError) {
                        elementError.textContent = '';
                    }
                }
                if (name_1) {
                    if (ignore != null && ignore.filter(function (x) { return x == name_1; }).length > 0) {
                        return "continue";
                    }
                    getObject(data, name_1, value);
                }
            };
            for (var i = 0; i < elements.length; ++i) {
                _loop_1(i);
            }
            data = removeNulls(data);
            return JSON.stringify(data);
        }
        exports.FormToJSONString = FormToJSONString;
        function GetParameterByName(name, url) {
            if (url === void 0) { url = null; }
            if (!url)
                url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'), results = regex.exec(url);
            if (!results)
                return null;
            if (!results[2])
                return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
        exports.GetParameterByName = GetParameterByName;
        function MapListToSelect2Data(render, fluentAggregation, data) {
            var display = fluentAggregation.Display;
            var externalKeys = fluentAggregation.ExternalKeys;
            var res = {
                results: $.map(data.Data, function (item) {
                    var displayValue = item[display];
                    var externalKeyValue = new Array();
                    externalKeys.forEach(function (key) {
                        var value = item[key];
                        value = ReplaceAll(value.toString(), '^', '|');
                        externalKeyValue.push(value);
                    });
                    var value = KeysToString(externalKeyValue);
                    return {
                        text: displayValue,
                        id: value
                    };
                })
            };
            return res;
        }
        exports.MapListToSelect2Data = MapListToSelect2Data;
        function Sucess(render, msg) {
            render.MessageService.Sucess(msg);
        }
        exports.Sucess = Sucess;
        function Exception(render, msg, state) {
            if (state === void 0) { state = -1; }
            if (render != null) {
                render.OperationsCallback("SaveError-" + msg, render);
            }
            if ((msg == '' || msg == undefined) && state == 0) {
                msg = Globalization_2.G('Server connection fail');
            }
            var component = render == null ? null : render.RootElement.querySelector("[name='message-error']");
            if (component == null) {
                throw "Error! " + msg;
            }
            render.MessageService.Error(msg);
        }
        exports.Exception = Exception;
        function IsJsonString(str) {
            try {
                JSON.parse(str);
            }
            catch (e) {
                return false;
            }
            return true;
        }
        exports.IsJsonString = IsJsonString;
        function GetParams() {
            var url = window.location.href;
            var params = [];
            var parser = document.createElement('a');
            parser.href = url;
            var query = parser.search.substring(1);
            var vars = query.split('&');
            var _loop_2 = function (i) {
                var pair = vars[i].split('=');
                var key = pair[0];
                var value = decodeURIComponent(pair[1]);
                var existent = params.filter(function (x) { return x.Key == key; })[0];
                if (existent == null) {
                    params.push(new KeyValues_1.KeyValues(key, value));
                }
                else {
                    existent.addValue(value);
                }
            };
            for (var i = 0; i < vars.length; i++) {
                _loop_2(i);
            }
            return params;
        }
        exports.GetParams = GetParams;
        ;
        function KeysToString(keys) {
            return keys.join('^');
        }
        exports.KeysToString = KeysToString;
        function StringToKeys(text) {
            return text.split('^');
        }
        exports.StringToKeys = StringToKeys;
        function KeysToQueryString(keyNames, keyValues) {
            var query = '';
            for (var i = 0; i < keyNames.length; i++) {
                var name_2 = keyNames[i];
                var value = keyValues[i];
                query += "&kn=" + name_2 + "&kv=" + value;
            }
            query.replace('&', '');
            return query;
        }
        exports.KeysToQueryString = KeysToQueryString;
        function KeysToFilters(render, keys, values) {
            if (keys.length != values.length) {
                Exception(render, "Number of keys does not match number of key values");
                return;
            }
            var filters = [];
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                var value = values[i];
                var filter = {
                    FilterType: EnumFilterType_1.EnumFilterType.EQUAL,
                    JunctionType: EnumJunctionType_1.EnumJunctionType.AND,
                    Value: value,
                    PropertyName: key,
                    Including: true,
                    IsReverse: false
                };
                filters.push(filter);
            }
            return JSON.stringify(filters);
        }
        exports.KeysToFilters = KeysToFilters;
        function CreateElementFromHTML(render, htmlString) {
            var div = document.createElement('div');
            div.innerHTML = htmlString.trim();
            return div.firstChild;
        }
        exports.CreateElementFromHTML = CreateElementFromHTML;
        function VariableTransformer(render, component, schema, keyValueDictionaty) {
            if (keyValueDictionaty === void 0) { keyValueDictionaty = []; }
            var newComponent = component;
            var propertyNames = Object.keys(schema);
            propertyNames.forEach(function (name) {
                var value = schema[name];
                keyValueDictionaty.forEach(function (dic) {
                    newComponent = ReplaceAll(newComponent, "{(" + dic.Key + ")}", dic.Value);
                });
                newComponent = ReplaceAll(newComponent, "__PropertyName__", schema['PropertyName']);
                newComponent = ReplaceAll(newComponent, "{(" + name + ")}", value);
            });
            return CreateElementFromHTML(render, newComponent);
        }
        exports.VariableTransformer = VariableTransformer;
        function GetElByNameOnAnotherElement(render, element, name) {
            if (element == null) {
                Exception(render, "In an attempt to search for the " + name + " element, the element where the search would be done is null.");
                return;
            }
            var elementFound = element.querySelectorAll("[name='" + name + "']");
            if (elementFound.length == 0) {
                Exception(render, FormatString(Globalization_2.G("Element not found with name {0}"), name));
                return null;
            }
            if (elementFound.length > 1) {
                Exception(render, "More than one element was found with name " + name);
                return null;
            }
            return elementFound[0];
        }
        exports.GetElByNameOnAnotherElement = GetElByNameOnAnotherElement;
        function Clone(obj) {
            return JSON.parse(JSON.stringify(obj));
        }
        exports.Clone = Clone;
        function GetElById(render, id) {
            var elementFound = render.FormComponent.querySelector("#" + id);
            if (elementFound == null) {
                Exception(render, FormatString(Globalization_2.G("Element not found with id {0}"), id));
            }
            return elementFound;
        }
        exports.GetElById = GetElById;
        function GetElTemplate(render, name) {
            var elementFound = render.Template.querySelector("[name=" + name + "]");
            if (elementFound == null) {
                Exception(render, FormatString(Globalization_2.G("Element not found with name {0}"), name));
            }
            return elementFound;
        }
        exports.GetElTemplate = GetElTemplate;
        function getTemplate(render, form) {
            var value = form;
            if (isNaN(Number(form))) {
                value = form;
            }
            else {
                value = EnumForm_1.EnumForm[form];
            }
            var name = "template-" + value.toString();
            return GetElTemplate(render, name);
        }
        exports.getTemplate = getTemplate;
        function AddScript(filepath, ignore, isLib, callback) {
            if (ignore) {
                callback();
                return;
            }
            if (filepath) {
                if (isLib == false && CONSTANT_3.CONSTANT.USE_CACHE == false) {
                    filepath += "?cache-disabled=" + CONSTANT_3.CONSTANT.SCRIPT_CACHE_CANCELATION;
                }
                if (document.querySelectorAll("script[src=\"" + filepath + "\"]").length > 0) {
                    callback();
                    return;
                }
                var fileref = document.createElement('script');
                fileref.onload = callback;
                fileref.setAttribute("type", "text/javascript");
                fileref.setAttribute("src", filepath);
                if (typeof fileref != "undefined") {
                    document.getElementsByTagName("head")[0].appendChild(fileref);
                }
            }
        }
        exports.AddScript = AddScript;
        function AddCss(filepath, ignore, isLib, callback) {
            if (ignore) {
                callback();
                return;
            }
            if (filepath) {
                if (isLib == false && CONSTANT_3.CONSTANT.USE_CACHE == false) {
                    filepath += "?cache-disabled=" + CONSTANT_3.CONSTANT.SCRIPT_CACHE_CANCELATION;
                }
                if (document.querySelectorAll("link[src=\"" + filepath + "\"]").length > 0) {
                    callback();
                    return;
                }
                var fileref = document.createElement('link');
                fileref.onload = callback;
                fileref.setAttribute("type", "text/css");
                fileref.setAttribute("rel", "stylesheet");
                fileref.setAttribute("href", filepath);
                if (typeof fileref != "undefined") {
                    document.getElementsByTagName("head")[0].appendChild(fileref);
                }
            }
        }
        exports.AddCss = AddCss;
        function LoadInitialScripts(render, userDefaultThema, completed) {
            if (render != null && render.IsPrincipal == false) {
                completed();
                return;
            }
            var quant = 7;
            function callbackCount() {
                quant--;
                if (quant == 0) {
                    AddScript(CONSTANT_3.CONSTANT.URL_JS_BOOTSTRAP, false, true, function () {
                        AddScript(CONSTANT_3.CONSTANT.URL_JS_SELECT2, false, true, function () {
                            completed();
                        });
                    });
                }
            }
            AddCss(CONSTANT_3.CONSTANT.URL_CSS_BOOTSTRAP, false, true, callbackCount);
            AddCss(CONSTANT_3.CONSTANT.URL_CSS_SELECT2, false, true, callbackCount);
            AddCss(CONSTANT_3.CONSTANT.URL_CSS_SELECT2_BOOTSTRAP, false, true, callbackCount);
            AddCss(CONSTANT_3.CONSTANT.URL_COMPONENTS_CSS, false, false, callbackCount);
            AddCss(CONSTANT_3.CONSTANT.URL_MAIN_CSS, !userDefaultThema, false, callbackCount);
            AddScript(CONSTANT_3.CONSTANT.URL_JS_JQUERY, false, true, callbackCount);
            AddScript(CONSTANT_3.CONSTANT.URL_JS_POPPER, false, true, callbackCount);
        }
        exports.LoadInitialScripts = LoadInitialScripts;
    });
    define("Enum/EnumFormKeyValue", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var EnumFormKeyValue = /** @class */ (function () {
            function EnumFormKeyValue(key, value) {
                this.Key = key;
                this.Value = value;
            }
            return EnumFormKeyValue;
        }());
        exports.EnumFormKeyValue = EnumFormKeyValue;
    });
    define("Render/Select2Render", ["require", "exports", "Util/Util", "Model/CONSTANT", "Util/Globalization"], function (require, exports, UTIL, CONSTANT_4, Globalization_3) {
        "use strict";
        exports.__esModule = true;
        function SetSelect2Value(render, jsonSchema, parentHtmlElement, data, property, element, isComposition, compositionPropertyName) {
            var schemaEl = jsonSchema.Properties.filter(function (x) { return x.PropertyName == property; })[0];
            var localKeys = schemaEl.FluentAggregation.LocalKeys;
            var externalKeys = schemaEl.FluentAggregation.ExternalKeys;
            var display = schemaEl.FluentAggregation.Display;
            var propertyName = schemaEl.FluentAggregation.PropertyName;
            var values = [];
            var select = $(element);
            var text = '';
            var id = '';
            var propertyData = data[propertyName];
            if (propertyData == null) {
                localKeys.forEach(function (key) {
                    var value = data[key];
                    values.push(value);
                });
                var jsonFilter = UTIL.KeysToFilters(render, externalKeys, values);
                // API.FindOneByFilter(render, propertyName, jsonFilter, (selectData) => {
                //     text = selectData[display];
                //     id = UTIL.KeysToString(values);
                //     select.append(new Option(text, id, false, true));
                //     select2Select(render, parentHtmlElement, localKeys, id, isComposition, compositionPropertyName);
                // });
            }
            else {
                externalKeys.forEach(function (key) {
                    var value = propertyData[key];
                    values.push(value);
                });
                text = propertyData[display];
                id = UTIL.KeysToString(values);
                select.append(new Option(text, id, false, true));
                select2Select(render, parentHtmlElement, localKeys, id, isComposition, compositionPropertyName);
            }
        }
        exports.SetSelect2Value = SetSelect2Value;
        function select2Event(render, parentHtmlElement, select, element, isComposition, compositionPropertyName) {
            $(select).on('select2:select', function (e) {
                var localKeys = element.FluentAggregation.LocalKeys;
                select2Select(render, parentHtmlElement, localKeys, e.params.data.id, isComposition, compositionPropertyName);
            });
        }
        exports.select2Event = select2Event;
        function ReloadSelect2(elementParent) {
            elementParent.querySelectorAll('.select2').forEach(function (x) { return x.style.width = '100%'; });
        }
        exports.ReloadSelect2 = ReloadSelect2;
        function select2Select(render, parentHtmlElement, localKeys, id, isComposition, compositionPropertyName) {
            var keyValues = UTIL.StringToKeys(id);
            for (var i = 0; i < keyValues.length; i++) {
                var value = keyValues[i];
                var key = localKeys[i];
                var fieldName = key;
                if (isComposition) {
                    fieldName = compositionPropertyName + "." + key;
                }
                var component = UTIL.GetElByNameOnAnotherElement(render, parentHtmlElement, fieldName);
                component.value = value;
            }
        }
        exports.select2Select = select2Select;
        function select2AutomaticList(render, select, fluentAggregation) {
            var url = UTIL.FormatString(CONSTANT_4.CONSTANT.URL_DATA_FIND_BY_PROXIMITY, fluentAggregation.Type);
            var propertyForFindByProximity = fluentAggregation.PropertyForFindByProximity;
            if (propertyForFindByProximity == null) {
                UTIL.Exception(render, 'NotImplementedException');
            }
            $(select).select2({
                placeholder: Globalization_3.G("Select"),
                language: {
                    searching: function () {
                        return Globalization_3.G('Finding...');
                    },
                    noResults: function () {
                        return Globalization_3.G('No results');
                    }
                },
                ajax: {
                    url: url,
                    dataType: "json",
                    type: "GET",
                    delay: 250,
                    data: function (params) {
                        var queryParameters = {
                            property: propertyForFindByProximity,
                            tolerance: CONSTANT_4.CONSTANT.PROXIMITY_FIND_TOLERANCE,
                            term: params.term
                        };
                        return queryParameters;
                    },
                    processResults: function (data) {
                        return UTIL.MapListToSelect2Data(render, fluentAggregation, data);
                    },
                    error: function (data) {
                        UTIL.Exception(render, data.responseText, data.status);
                    }
                }
            });
        }
        exports.select2AutomaticList = select2AutomaticList;
    });
    define("Render/FormRender", ["require", "exports", "Enum/EnumForm", "Util/Util", "Enum/EnumFormKeyValue", "Model/EnumGrupType", "Render/Select2Render", "Model/CONSTANT"], function (require, exports, EnumForm_2, UTIL, EnumFormKeyValue_1, EnumGrupType_1, SELECT2_RENDER, CONSTANT_5) {
        "use strict";
        exports.__esModule = true;
        function CreateCompositionTab(render, property, localToRender, isPrincipal) {
            var _this = this;
            var tabControolHtml = "<ul class=\"fluent-tab-controll\" name=\"fluent-tab-controll\"></ul>";
            var tabContentHtml = "<div name='fluent-tab-content' class='fluent-tab-content'></div>";
            var tabControol = UTIL.CreateElementFromHTML(render, tabControolHtml);
            var tabContent = UTIL.CreateElementFromHTML(render, tabContentHtml);
            var fluentJsonSchema = property.FluentComposition.Form;
            var name = property.Name;
            var element;
            if (isPrincipal) {
                localToRender.appendChild(tabControol);
                localToRender.appendChild(tabContent);
                element = AddTab(render, tabControol, tabContent, name, true, property.IsList);
                if (property.IsList) {
                    var tabHtml = "\n    <li class=\"nav-item\">\n        <img style=\"cursor:pointer; height:11px;\" src=\"" + CONSTANT_5.CONSTANT.URL_IMG_ADD + "\">\n    </li>";
                    var addImageTagIcon_1 = UTIL.CreateElementFromHTML(render, tabHtml);
                    addImageTagIcon_1.querySelector('img').addEventListener('click', function (e) {
                        tabControol.querySelectorAll('.selected').forEach(function (element) {
                            element.classList.remove('selected');
                        });
                        tabContent.querySelectorAll('.selected').forEach(function (element) {
                            element.classList.remove('selected');
                            var htmlElement = element;
                            htmlElement.hidden = true;
                        });
                        var tabcontent = AddTab(render, tabControol, tabContent, name, true, property.IsList);
                        var propertyName = property.PropertyName;
                        if (property.IsList) {
                            propertyName += "[" + CONSTANT_5.CONSTANT.NextArrayValue() + "]";
                        }
                        _this.RenderForm.call(_this, render, fluentJsonSchema, tabcontent, true, propertyName);
                        tabControol.appendChild(addImageTagIcon_1);
                    });
                    tabControol.appendChild(addImageTagIcon_1);
                }
            }
            else {
                element = localToRender;
            }
            var propertyName = property.PropertyName;
            if (property.IsList) {
                propertyName += "[" + CONSTANT_5.CONSTANT.NextArrayValue() + "]";
            }
            _this.RenderForm(render, fluentJsonSchema, element, true, propertyName);
        }
        exports.CreateCompositionTab = CreateCompositionTab;
        function RenderForm(render, fluentJsonSchema, localToRender, isComposition, compositionPropertyName) {
            if (compositionPropertyName === void 0) { compositionPropertyName = null; }
            var templates = $.map(EnumForm_2.EnumForm, function (x) { return new EnumFormKeyValue_1.EnumFormKeyValue(x, UTIL.getTemplate(render, x)); });
            var _this = this;
            var row;
            var lastRowNum = 1;
            var lastGroupName = null;
            var group = null;
            var UseGroups = fluentJsonSchema.Properties.filter(function (x) { return x.Group != fluentJsonSchema.Properties[0].Group; }).length > 1;
            var tabControolHtml = "<ul class=\"fluent-tab-controll\" name=\"fluent-tab-controll\"></ul>";
            var tabContentHtml = "<div name='fluent-tab-content' class='fluent-tab-content'></div>";
            var tabControol = UTIL.CreateElementFromHTML(render, tabControolHtml);
            var tabContent = UTIL.CreateElementFromHTML(render, tabContentHtml);
            var keyValueDictionary = [];
            keyValueDictionary.push({ Key: 'IconAdd', Value: CONSTANT_5.CONSTANT.URL_IMG_ADD });
            if (UseGroups) {
                localToRender.appendChild(tabControol);
            }
            localToRender.appendChild(tabContent);
            if (render.IsEdit) {
                render.FormContainerComponent.classList.add('fluent-edition');
            }
            fluentJsonSchema.Properties.forEach(function (property) {
                var component;
                var element;
                if (compositionPropertyName != null) {
                    keyValueDictionary.push({ Key: 'PropertyName', Value: compositionPropertyName + ".__PropertyName__" });
                }
                if (property.IsEnum) {
                    var name_3 = "template-COMBOBOX-enum";
                    component = UTIL.GetElTemplate(render, name_3);
                    element = UTIL.VariableTransformer(render, component.innerHTML, property, keyValueDictionary);
                }
                else if (property.FluentComposition == null) {
                    component = templates.filter(function (x) { return x.Key == property.Form; })[0].Value;
                    element = UTIL.VariableTransformer(render, component.innerHTML, property, keyValueDictionary);
                }
                else {
                    property.Group = property.Name;
                    CreateCompositionTab.call(_this, render, property, group, true);
                    return;
                }
                if (group == null) {
                    var groupName = property.Group == null ? fluentJsonSchema.FluentJsonForm.Name : property.Group;
                    if (!UseGroups) {
                        group = AddDiv(render, localToRender);
                    }
                    else if (property.GroupType.toString() == EnumGrupType_1.EnumGrupType[EnumGrupType_1.EnumGrupType.TAB]) {
                        group = AddTab(render, tabControol, tabContent, groupName, true, false);
                    }
                    else {
                        group = AddGroup(render, groupName, localToRender);
                    }
                    lastGroupName = property.Group;
                }
                ;
                if (property.Group != lastGroupName) {
                    var groupName = property.Group == null ? fluentJsonSchema.FluentJsonForm.Name : property.Group;
                    if (!UseGroups) {
                        group = AddDiv(render, localToRender);
                    }
                    else if (property.GroupType.toString() == EnumGrupType_1.EnumGrupType[EnumGrupType_1.EnumGrupType.TAB]) {
                        group = AddTab(render, tabControol, tabContent, groupName, false, false);
                    }
                    else {
                        group = AddGroup(render, groupName, localToRender);
                    }
                }
                var htmlElements = element.querySelectorAll("input, select, textarea");
                if (render.IsEdit && property.IsKey) {
                    htmlElements.forEach(function (c) {
                        var localElement = c;
                        localElement.disabled = true;
                        localElement.readOnly = true;
                    });
                }
                if (row == null) {
                    row = AddRow(render, group);
                }
                ;
                if (property.Form.toString() == EnumForm_2.EnumForm[EnumForm_2.EnumForm.HIDDEN]) {
                    row.appendChild(element);
                    return;
                }
                if (property.Row != lastRowNum) {
                    row = AddRow(render, group);
                    lastRowNum = property.Row;
                }
                if (property.IsEnum) {
                    AddEnums(render, property, element);
                }
                row.appendChild(element);
                htmlElements.forEach(function (x) {
                    var el = x;
                    if (property.DefaultValue != '') {
                        el.value = property.DefaultValue;
                    }
                });
            });
            render.LoadAggregations(render, localToRender, fluentJsonSchema, isComposition, compositionPropertyName);
            return fluentJsonSchema;
        }
        exports.RenderForm = RenderForm;
        function AddTab(render, tabControol, tabContent, groupName, first, isList) {
            var _this = this;
            var tabHtml = isList ? "\n      <li class=\"nav-item\">\n          <img style=\"cursor:pointer; height:11px;\" src=\"" + CONSTANT_5.CONSTANT.URL_IMG_REMOVE + "\">\n          <span>" + groupName + "</span>\n      </li>" : "\n      <li class=\"nav-item\">\n          <span>" + groupName + "</span>\n      </li>";
            var contentHtml = "<div class='fluent-tab-content-element'></div>";
            var tabControllTab = UTIL.CreateElementFromHTML(render, tabHtml);
            var tabContentTab = UTIL.CreateElementFromHTML(render, contentHtml);
            var tabIconRemove = tabControllTab.querySelector('img');
            if (tabIconRemove != null) {
                tabIconRemove.addEventListener('click', function (e) {
                    var firstContent = tabControllTab.parentElement.childNodes[0];
                    var firstTab = tabContentTab.parentElement.childNodes[0];
                    removeSelect();
                    selectTab(firstTab, firstContent);
                    tabControllTab.remove();
                    tabContentTab.remove();
                });
            }
            if (first) {
                tabControllTab.classList.add('selected');
                tabContentTab.classList.add('selected');
            }
            tabControllTab.addEventListener('click', function (e) {
                if (e.target != null && e.target.tagName == 'IMG') {
                    return;
                }
                removeSelect();
                selectTab(tabControllTab, tabContentTab);
            });
            function removeSelect() {
                tabControol.querySelectorAll('.selected').forEach(function (element) {
                    element.classList.remove('selected');
                });
                tabContent.querySelectorAll('.selected').forEach(function (element) {
                    element.classList.remove('selected');
                    var htmlElement = element;
                    htmlElement.hidden = true;
                });
            }
            tabControol.appendChild(tabControllTab);
            tabContent.appendChild(tabContentTab);
            return tabContentTab;
        }
        exports.AddTab = AddTab;
        function selectTab(tabControllTab, tabContentTab) {
            tabControllTab.classList.add('selected');
            tabContentTab.classList.add('selected');
            tabControllTab.hidden = false;
            tabContentTab.hidden = false;
            var htmlElement = tabContentTab;
            htmlElement.hidden = false;
            SELECT2_RENDER.ReloadSelect2(htmlElement);
        }
        function AddEnums(render, property, element) {
            var options = $.map(property.Enums, function (keyValue) {
                var option = document.createElement('option');
                option.id = keyValue.Key;
                option.text = keyValue.Value;
                return option;
            });
            var select = element.querySelector('select');
            options.forEach(function (x) { return select.add(x); });
        }
        exports.AddEnums = AddEnums;
        function AddRow(render, group) {
            var row = document.createElement('div');
            row.classList.add('row');
            group.appendChild(row);
            return row;
        }
        exports.AddRow = AddRow;
        function AddGroup(render, groupName, localElement) {
            var group = UTIL.CreateElementFromHTML(render, "<div class='group'><h6 class=\"group-title\">" + groupName + "</h6></div>");
            localElement.appendChild(group);
            return group;
        }
        exports.AddGroup = AddGroup;
        function AddDiv(render, localElement) {
            var div = UTIL.CreateElementFromHTML(render, "<div></div>");
            localElement.appendChild(div);
            return div;
        }
        exports.AddDiv = AddDiv;
    });
    define("Model/Versions", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Tests/RenderTestClass", ["require", "exports", "API/API", "Util/Util", "Model/CONSTANT"], function (require, exports, API, UTIL, CONSTANT_6) {
        "use strict";
        exports.__esModule = true;
        var RenderTestClass = /** @class */ (function () {
            function RenderTestClass(parameters) {
                this.sucessCount = 0;
                this.errorsCount = 0;
                this.i = 0;
                this.isPrincipal = true;
                this.stop = false;
                {
                    if (CONSTANT_6.CONSTANT.USE_CACHE == false) {
                        console.warn('Attention! Cache control is disabled by the application.');
                    }
                    CONSTANT_6.CONSTANT.SetParameters(parameters);
                    this.Parameters = parameters;
                    this.OperationsCallback = parameters.Callback;
                    var rootOfRoot = document.getElementById(this.Parameters.RootElementId);
                    rootOfRoot.textContent = '';
                    {
                        this.RootTestElement = document.createElement('div');
                        this.RootTestElement.classList.add('root-test');
                        rootOfRoot.appendChild(this.RootTestElement);
                    }
                    {
                        this.FormsContanierElement = document.createElement('div');
                        this.FormsContanierElement.id = 'root-for-forms';
                        rootOfRoot.appendChild(this.FormsContanierElement);
                    }
                    {
                        this.TitleComponent = document.createElement('span');
                        this.TitleComponent.classList.add('title');
                    }
                    {
                        this.TitleLabel = document.createElement('span');
                        this.TitleLabel.classList.add('title-label');
                        this.TitleComponent.appendChild(this.TitleLabel);
                    }
                    {
                        this.TitleSucess = document.createElement('span');
                        this.TitleSucess.classList.add('title-sucess');
                        this.TitleComponent.appendChild(this.TitleSucess);
                    }
                    {
                        this.TitleErrors = document.createElement('span');
                        this.TitleErrors.classList.add('title-errors');
                        this.TitleComponent.appendChild(this.TitleErrors);
                    }
                    this.OperationsComponent = document.createElement('div');
                    this.OperationsComponent.classList.add('operations');
                    this.RootTestElement.appendChild(this.TitleComponent);
                    this.RootTestElement.appendChild(this.OperationsComponent);
                    if (UTIL.GetParams().filter(function (x) { return x.Key == 'time'; })[0] == null) {
                        this.Time = 1;
                    }
                    else {
                        var time = UTIL.GetParams().filter(function (x) { return x.Key == 'time'; })[0].Values[0];
                        this.Time = Number(time);
                    }
                }
            }
            RenderTestClass.prototype.StartTests = function () {
                this.SetTitleText('Starting integration tests...');
                this.Steps();
            };
            RenderTestClass.prototype.SetTitleText = function (text) {
                this.TitleLabel.textContent = text;
            };
            RenderTestClass.prototype.Steps = function () {
                var _this_1 = this;
                var _this = this;
                this.SetTitleText('Getting data from the server...');
                this.GetServerData(function (data) {
                    _this.data = data;
                    _this_1.SetTitleText(data.length + " tests found");
                    data.forEach(function (node) {
                        _this.addTestElement(node);
                    });
                    _this.startTest();
                });
            };
            RenderTestClass.prototype.startTest = function () {
                var _this = this;
                if (_this.stop) {
                    return;
                }
                if (_this.data.length < _this.i) {
                    return;
                }
                var node = _this.data[_this.i];
                _this.i++;
                _this.CallRender(node, true);
            };
            RenderTestClass.prototype.CallRender = function (node, runing) {
                var _this = this;
                function SavedSuccessfully() {
                    render.Dispose(render);
                    _this.setTestResult(node, true, '');
                    if (runing) {
                        _this.startTest.call(_this, false);
                    }
                }
                var parameters = UTIL.Clone(_this.Parameters);
                parameters.FormName = node.EntityTypeName;
                parameters.RootElementId = 'root-for-forms';
                parameters.IsPrincipal = _this.isPrincipal;
                parameters.Template = _this.template;
                parameters.Callback = Callback;
                var render = Render.Render(parameters);
                _this.Render = render;
                render.Execute();
                _this.isPrincipal = false;
                function Callback(message, render_test) {
                    if (message == 'FormLoaded') {
                        _this.template = render_test.Template;
                        render_test.LoadDataForEditByData(render_test, node.Instance);
                        if (runing) {
                            render_test.SubmitButton.click();
                        }
                    }
                    else if (message == 'SavedSuccessfully') {
                        if (_this.Time == 1) {
                            SavedSuccessfully();
                        }
                        else {
                            setTimeout(function () { SavedSuccessfully(); }, _this.Time);
                        }
                    }
                    else if (message.indexOf('SaveError') >= 0 && runing) {
                        setTimeout(function () {
                            if (runing) {
                                render_test.Dispose(render_test);
                                _this.setTestResult(node, false, '');
                                _this.startTest.call(_this, false);
                            }
                        }, _this.Time);
                    }
                    if (runing) {
                        clearTimeout(timeOutCall);
                    }
                    ;
                }
                if (runing) {
                    var timeOutCall = setTimeout(function () {
                        render.Dispose(render);
                        _this.setTestResult(node, false, '');
                        _this.startTest.call(_this, false);
                    }, 1000);
                }
            };
            RenderTestClass.prototype.addTestElement = function (node) {
                var _this = this;
                var test = document.createElement('span');
                test.classList.add('test', "test-" + node.EntityTypeName);
                test.textContent = node.EntityTypeName;
                test.addEventListener('click', function (e) {
                    _this.stop = true;
                    if (Render != null) {
                        _this.Render.Dispose(_this.Render);
                    }
                    _this.CallRender(node, false);
                });
                this.OperationsComponent.appendChild(test);
            };
            RenderTestClass.prototype.setTestResult = function (node, sucess, message) {
                var el = this.OperationsComponent.querySelector(".test-" + node.EntityTypeName);
                if (sucess) {
                    el.classList.add('sucess');
                    this.sucessCount++;
                    this.TitleSucess.textContent = this.sucessCount + " sucess";
                }
                else {
                    el.classList.add('fail');
                    this.errorsCount++;
                    this.TitleErrors.textContent = this.errorsCount + " errors";
                }
            };
            RenderTestClass.prototype.GetServerData = function (callback) {
                var url = CONSTANT_6.CONSTANT.URL_DATA_FOR_TEST;
                API.GetDataFromServer(null, url, function (data) {
                    callback(data);
                });
            };
            return RenderTestClass;
        }());
        exports.RenderTestClass = RenderTestClass;
    });
    define("Filter/FilterModel", ["require", "exports", "Enum/Filter"], function (require, exports, Filter_1) {
        "use strict";
        exports.__esModule = true;
        var FilterModel = /** @class */ (function (_super) {
            __extends(FilterModel, _super);
            function FilterModel() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            return FilterModel;
        }(Filter_1.Filter));
        exports.FilterModel = FilterModel;
    });
    define("Filter/RenderFilterClass", ["require", "exports", "API/API", "Util/Util", "Enum/EnumForm", "Filter/FilterModel", "Enum/EnumFilterType", "Enum/EnumJunctionType", "Util/Globalization"], function (require, exports, API, UTIL, EnumForm_3, FilterModel_1, EnumFilterType_2, EnumJunctionType_2, Globalization_4) {
        "use strict";
        exports.__esModule = true;
        var RenderFilterClass = /** @class */ (function () {
            function RenderFilterClass(form_name, root_element_id, callBack) {
                this.Filters = [];
                var _this = this;
                this.Type = form_name;
                this.RootElement = document.createElement('div');
                var root = document.getElementById(root_element_id); // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                if (root == null) {
                    throw root_element_id + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                this.CallBack = callBack;
                root.appendChild(this.RootElement);
                this.RootElement = document.getElementById(root_element_id); // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                if (this.RootElement == null) {
                    throw root_element_id + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                var html = "\n<div class=\"filter-group\">\n    <div class=\"title\">\n        <span>" + Globalization_4.G('Filters') + "</span>\n        <span class='minimize' name='minimize' title=\"" + Globalization_4.G('Click to minimize') + "\">-</span>\n    </div>\n\n    <div class=\"title-maximize\" name=\"title-maximize\" title=\"" + Globalization_4.G('Click to maximize') + "\">\n        <span>" + Globalization_4.G('Filters') + "</span>\n    </div>\n\n    <div class=\"row\">\n        <div class=\"form-group col-12\">\n            <label for=\"property\">" + Globalization_4.G('Field') + "</label>\n            <select class=\"form-control select2\" name=\"property\"></select>\n            <span class=\"help-block field-error\" name=\"error-property\"></span>\n        </div>\n    </div>\n\n    <div class=\"row\">\n\n        <div class=\"form-group col-12\">\n            <label for=\"filter-type\">" + Globalization_4.G('Filter type') + "</label>\n            <div class=\"form-check\" style=\"display: inline;\">\n                <input type=\"checkbox\" class=\"form-check-input\" name=\"revert-type\" id=\"revert-type\">\n                <label class=\"form-check-label\" for=\"revert-type\">" + Globalization_4.G('Revert') + "</label>\n            </div>\n            <select class=\"form-control\" name=\"filter-type\">\n            </select>\n            <span class=\"help-block field-error\" name=\"error-filter-type\"></span>\n        </div>\n    </div>\n\n    <div class=\"row\">\n        <div class=\"form-group col-12\">\n            <label for=\"filter-type\">" + Globalization_4.G('Value') + "</label>\n            <input type=\"text\" class=\"form-control\" name=\"value-textbox\" placeholder=\"" + Globalization_4.G('Value') + "\">\n        </div>\n    </div>\n\n    <div class=\"row\">\n        <div class=\"form-group col-12\">\n            <label for=\"filter-type\">" + Globalization_4.G('Value') + "</label>\n            <select class=\"form-control\" name=\"value-enum\">\n            </select>\n        </div>\n\n        <div class=\"form-group col-12\">\n            <label for=\"filter-type\">" + Globalization_4.G('Value') + "</label>\n            <select class=\"form-control select2\" name=\"value-select2\">\n            </select>\n        </div>\n\n        <div class=\"form-group col-12\">\n            <label for=\"value-date\">" + Globalization_4.G('Value') + "</label>\n            <input type=\"date\" class=\"form-control\" name=\"value-date\">\n        </div>\n\n        <div class=\"form-group col-12\">\n            <label for=\"value-number\">" + Globalization_4.G('Value') + "</label>\n            <input type=\"number\" class=\"form-control\" name=\"value-number\">\n        </div>\n    </div>\n\n    <div class=\"row\">\n        <div class=\"form-group col-12\">\n            <button type=\"button\" name=\"addButton\" class=\"button-add btn btn-primary submitButton\">" + Globalization_4.G('Add') + "</button>\n        </div>\n    </div>\n\n    <div class=\"row\">\n        <div class=\"form-group col-12\">\n            <ul class=\"filters-container\" name=\"filters-container\"></ul>\n        </div>\n    </div>\n</div>\n            ";
                var filterComponent = UTIL.CreateElementFromHTML(null, html);
                var minimize = UTIL.GetElByNameOnAnotherElement(null, filterComponent, 'minimize');
                var title_maximize = UTIL.GetElByNameOnAnotherElement(null, filterComponent, 'title-maximize');
                minimize.addEventListener('click', function (e) {
                    filterComponent.classList.add('filter-minimized');
                    _this.Event.call(_this, 'minimize', null);
                });
                title_maximize.addEventListener('click', function (e) {
                    filterComponent.classList.remove('filter-minimized');
                    _this.Event.call(_this, 'maximize', null);
                });
                this.RootElement.appendChild(filterComponent);
                this.PropertyComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'property');
                this.FilterTypeComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'filter-type');
                this.RevertTypeComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'revert-type');
                this.AddButton = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'addButton');
                this.ValueTextboxComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'value-textbox');
                this.ValueEnumComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'value-enum');
                this.ValueSelect2Component = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'value-select2');
                this.ValueDateComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'value-date');
                this.ValueNumberComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'value-number');
                this.FiltersContainerComponent = UTIL.GetElByNameOnAnotherElement(null, this.RootElement, 'filters-container');
                this.ClearValues();
                this.StartEvents();
            }
            RenderFilterClass.prototype.StartEvents = function () {
                var _this = this;
                _this.PropertyComponent.addEventListener('change', function (e) {
                    _this.PropertySelected = _this.Schema.Properties.filter(function (x) { return x.PropertyName == _this.PropertyComponent.value; })[0];
                    _this.ShowOptions();
                    _this.FilterTypeComponent.dispatchEvent(new Event('change'));
                });
                _this.FilterTypeComponent.addEventListener('change', function (e) {
                    _this.FilterTypeSelected = EnumFilterType_2.EnumFilterType[Object.keys(EnumFilterType_2.EnumFilterType).filter(function (x) { return x == _this.FilterTypeComponent.value; })[0]];
                    _this.ShowValueComponent();
                });
                _this.AddButton.addEventListener('click', function (e) {
                    _this.AddNewFilter.call(_this);
                });
            };
            RenderFilterClass.prototype.Event = function (event, object) {
                this.CallBack(event, object);
            };
            RenderFilterClass.prototype.AddNewFilter = function () {
                var _this = this;
                var model = new FilterModel_1.FilterModel();
                model.Value = _this.GetValue(_this.PropertySelected);
                model.FilterType = EnumFilterType_2.EnumFilterType[_this.FilterTypeComponent.value];
                model.JunctionType = EnumJunctionType_2.EnumJunctionType.AND;
                model.PropertyName = _this.PropertyComponent.value;
                model.IsReverse = _this.RevertTypeComponent.checked;
                model.FieldName = _this.PropertySelected.Name;
                model.FilterName = _this.FilterTypeComponent.value;
                if ((model.Value == null || model.Value == '') && _this.FilterTypeComponent.value != 'NULL') {
                    alert(Globalization_4.G('Enter a value for the value field'));
                    return;
                }
                var labelOr = Globalization_4.G('OR');
                var labelAnd = Globalization_4.G('AND');
                var labelClickToChange = 'Click to change';
                var html = "\n<li>\n    <span class='field-remove'>x</span>\n    <span class='field-name'>" + model.FieldName + "</span>\n    <span class='filter-not'>" + (model.IsReverse ? Globalization_4.G('not') + '&nbsp;' : '') + "</span>\n    <span class='filter-name'>" + Globalization_4.G(model.FilterName) + "</span>\n    <span class='value'>" + model.Value + "</span>    \n</li>";
                var component = UTIL.CreateElementFromHTML(null, html);
                var junctionComponent;
                if (_this.FiltersContainerComponent.childNodes.length >= 1) {
                    var junction = "\n            <li class='field-junction field-junction-and'>  \n                <span data-junction='AND' title='" + labelClickToChange + "'>" + labelAnd + "</span>\n            </li>  \n                    ";
                    junctionComponent = UTIL.CreateElementFromHTML(null, junction);
                    _this.FiltersContainerComponent.appendChild(junctionComponent);
                    junctionComponent.addEventListener('click', function (e) {
                        var span = junctionComponent.querySelector('span');
                        var junc = span.getAttribute('data-junction');
                        if (junc == 'OR') {
                            span.setAttribute('data-junction', 'AND');
                            span.parentElement.classList.remove('field-junction-or');
                            span.parentElement.classList.add('field-junction-and');
                            span.innerText = labelAnd;
                            model.JunctionType = EnumJunctionType_2.EnumJunctionType.AND;
                        }
                        else {
                            span.setAttribute('data-junction', 'OR');
                            span.parentElement.classList.remove('field-junction-and');
                            span.parentElement.classList.add('field-junction-or');
                            span.innerText = labelOr;
                            model.JunctionType = EnumJunctionType_2.EnumJunctionType.OR;
                        }
                        _this.Event.call(_this, 'FlterChanged', _this.Filters);
                    });
                }
                component.addEventListener('click', function (e) {
                    var index = _this.Filters.indexOf(model);
                    _this.Filters.splice(index, 1);
                    component.remove();
                    if (junctionComponent != null) {
                        junctionComponent.remove();
                    }
                    var first = _this.FiltersContainerComponent.childNodes.item(0);
                    if (first != null && first.classList.contains('field-junction')) {
                        first.remove();
                    }
                    _this.Event.call(_this, 'FlterChanged', _this.Filters);
                });
                _this.FiltersContainerComponent.appendChild(component);
                _this.Filters.push(model);
                _this.Event.call(_this, 'FlterChanged', _this.Filters);
            };
            RenderFilterClass.prototype.ShowOptions = function () {
                var _this = this;
                _this.FilterTypeComponent.innerHTML = '';
                var options;
                switch (_this.PropertySelected.Form.toString()) {
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.TEXTBOX]:
                        {
                            options = [
                                EnumFilterType_2.EnumFilterType.CONTAINS,
                                EnumFilterType_2.EnumFilterType.EQUAL,
                                EnumFilterType_2.EnumFilterType.ENDS_WITH,
                                EnumFilterType_2.EnumFilterType.START_WITH
                            ];
                            break;
                        }
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.COMBOBOX]:
                        {
                            if (_this.PropertySelected.IsEnum) {
                                options = [
                                    EnumFilterType_2.EnumFilterType.EQUAL
                                ];
                            }
                            else {
                                options = [
                                    EnumFilterType_2.EnumFilterType.EQUAL
                                ];
                            }
                            break;
                        }
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.DATEPICKER]:
                        {
                            options = [
                                EnumFilterType_2.EnumFilterType.EQUAL,
                                EnumFilterType_2.EnumFilterType.GREATER,
                                EnumFilterType_2.EnumFilterType.SMALLER
                            ];
                            break;
                        }
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.NUMBER]:
                        {
                            options = [
                                EnumFilterType_2.EnumFilterType.EQUAL,
                                EnumFilterType_2.EnumFilterType.GREATER,
                                EnumFilterType_2.EnumFilterType.SMALLER
                            ];
                            break;
                        }
                }
                if (_this.PropertySelected.IsNullable) {
                    options.push(EnumFilterType_2.EnumFilterType.NULL);
                }
                options.forEach(function (op) {
                    var option = document.createElement('option');
                    option.text = Globalization_4.G(EnumFilterType_2.EnumFilterType[op]);
                    option.value = EnumFilterType_2.EnumFilterType[op];
                    _this.FilterTypeComponent.add(option);
                });
            };
            RenderFilterClass.prototype.ClearValues = function () {
                var _this = this;
                _this.ValueTextboxComponent.parentElement.hidden = true;
                _this.ValueEnumComponent.parentElement.hidden = true;
                _this.ValueSelect2Component.parentElement.hidden = true;
                _this.ValueDateComponent.parentElement.hidden = true;
                _this.ValueNumberComponent.parentElement.hidden = true;
                _this.ValueTextboxComponent.value = null;
                _this.ValueEnumComponent.value = null;
                _this.ValueSelect2Component.value = null;
                _this.ValueDateComponent.value = null;
                _this.ValueNumberComponent.value = null;
            };
            RenderFilterClass.prototype.ShowValueComponent = function () {
                var _this = this;
                _this.ClearValues();
                switch (EnumFilterType_2.EnumFilterType[_this.FilterTypeSelected.toString()]) {
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.ENDS_WITH]:
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.START_WITH]:
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.CONTAINS]:
                        {
                            _this.ValueTextboxComponent.parentElement.hidden = false;
                            ;
                            return;
                        }
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.FALSE]:
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.TRUE]:
                        {
                            return;
                        }
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.GREATER]:
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.SMALLER]:
                        {
                            if (_this.PropertySelected.Form.toString() == EnumForm_3.EnumForm[EnumForm_3.EnumForm.DATEPICKER]) {
                                _this.ValueDateComponent.parentElement.hidden = false;
                                ;
                                return;
                            }
                            else if (_this.PropertySelected.Form.toString() == EnumForm_3.EnumForm[EnumForm_3.EnumForm.NUMBER]) {
                                _this.ValueNumberComponent.parentElement.hidden = false;
                                ;
                                return;
                            }
                        }
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.NULL]: {
                        return;
                    }
                    case EnumFilterType_2.EnumFilterType[EnumFilterType_2.EnumFilterType.EQUAL]:
                        {
                            SelectByForm();
                            return;
                        }
                }
                function SelectByForm() {
                    switch (_this.PropertySelected.Form.toString()) {
                        case EnumForm_3.EnumForm[EnumForm_3.EnumForm.TEXTBOX]:
                            {
                                _this.ValueTextboxComponent.parentElement.hidden = false;
                                ;
                                return;
                            }
                        case EnumForm_3.EnumForm[EnumForm_3.EnumForm.COMBOBOX]:
                            {
                                if (_this.PropertySelected.IsEnum) {
                                    _this.ValueEnumComponent.innerHTML = '';
                                    _this.PropertySelected.Enums.forEach(function (e) {
                                        if (e.Key == 'none' || e.Key == 'None' || e.Key == 'Nenhum') {
                                            return;
                                        }
                                        var option = document.createElement('option');
                                        option.text = e.Value;
                                        option.value = e.Value;
                                        _this.ValueEnumComponent.add(option);
                                    });
                                    _this.ValueEnumComponent.parentElement.hidden = false;
                                    ;
                                    return;
                                }
                                else {
                                    _this.ValueSelect2Component.parentElement.hidden = false;
                                    ;
                                    return;
                                }
                            }
                        case EnumForm_3.EnumForm[EnumForm_3.EnumForm.DATEPICKER]:
                            {
                                _this.ValueDateComponent.parentElement.hidden = false;
                                ;
                                return;
                            }
                        case EnumForm_3.EnumForm[EnumForm_3.EnumForm.NUMBER]:
                            {
                                _this.ValueNumberComponent.parentElement.hidden = false;
                                ;
                                return;
                            }
                    }
                }
            };
            RenderFilterClass.prototype.GetValue = function (property) {
                var _this = this;
                switch (property.Form.toString()) {
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.TEXTBOX]:
                        {
                            return _this.ValueTextboxComponent.value;
                        }
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.COMBOBOX]:
                        {
                            if (property.IsEnum) {
                                return _this.ValueEnumComponent.value;
                            }
                            return _this.ValueSelect2Component.value;
                        }
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.DATEPICKER]:
                        {
                            return _this.ValueDateComponent.value;
                        }
                    case EnumForm_3.EnumForm[EnumForm_3.EnumForm.NUMBER]:
                        {
                            return _this.ValueNumberComponent.value;
                        }
                }
            };
            RenderFilterClass.prototype.StartFilter = function (userDefaultThema) {
                if (userDefaultThema === void 0) { userDefaultThema = true; }
                var _this = this;
                UTIL.LoadInitialScripts(null, userDefaultThema, function () {
                    API.GetJsonSchema(null, _this.Type, function (data) {
                        _this.Schema = data;
                        data.Properties.forEach(function (property) {
                            if (property.Form.toString() == EnumForm_3.EnumForm[EnumForm_3.EnumForm.HIDDEN]) {
                                return;
                            }
                            if (property.Form.toString() == EnumForm_3.EnumForm[EnumForm_3.EnumForm.FILE]) {
                                return;
                            }
                            if (property.Form.toString() == EnumForm_3.EnumForm[EnumForm_3.EnumForm.FORM]) {
                                return;
                            }
                            var option = document.createElement('option');
                            option.text = property.Name;
                            option.value = property.PropertyName;
                            _this.PropertyComponent.add(option);
                        });
                        _this.PropertyComponent.dispatchEvent(new Event('change'));
                        _this.Event.call(_this, 'FormLoaded', null);
                    });
                });
            };
            return RenderFilterClass;
        }());
        exports.RenderFilterClass = RenderFilterClass;
    });
    define("Aggregations/FluentManyToManyAggregationAttribute", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Aggregations/FluentManyToManyAggregationAttributeComplete", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
    });
    define("Aggregations/ManyNoMany", ["require", "exports"], function (require, exports) {
        "use strict";
        exports.__esModule = true;
        var ManyNoMany = /** @class */ (function () {
            function ManyNoMany() {
            }
            return ManyNoMany;
        }());
        exports.ManyNoMany = ManyNoMany;
    });
    define("Aggregations/RenderManyToManyAggregations", ["require", "exports", "API/API", "Util/Util", "Util/Globalization", "Model/CONSTANT"], function (require, exports, API, UTIL, Globalization_5, CONSTANT_7) {
        "use strict";
        exports.__esModule = true;
        var RenderManyToManyAggregations = /** @class */ (function () {
            function RenderManyToManyAggregations(form_name, entityOne, entityMany, root_element_id) {
                this.Filters = [];
                this.root_element_id = root_element_id;
                this.PrincipalType = form_name;
                this.EntityOne = entityOne;
                this.EntityMany = entityMany;
                this.RootElement = document.createElement('div');
                var root = document.getElementById(root_element_id); // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                if (root == null) {
                    throw root_element_id + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                if (this.RootElement == null) {
                    throw root_element_id + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                root.classList.remove('container');
                root.appendChild(this.RootElement);
            }
            RenderManyToManyAggregations.prototype.Start = function (userDefaultThema) {
                if (userDefaultThema === void 0) { userDefaultThema = true; }
                var _this = this;
                UTIL.LoadInitialScripts(null, userDefaultThema, function () {
                    API.GetJsonSchema(null, _this.EntityMany.Type, function (data) {
                        _this.Schema = data;
                        _this.RegiaterComponents();
                        _this.StartEvents.call(_this);
                    });
                });
            };
            RenderManyToManyAggregations.prototype.RegiaterComponents = function () {
                var _this = this;
                this.RenderHtmlBase();
                {
                    var availableGridComponent = UTIL.GetElByNameOnAnotherElement(null, _this.PrincipalComponent, 'available-grid');
                    this.AvailableRenderGridComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'grid-render-content');
                    this.AvailablesSearchButton = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'executeButton');
                    this.AvailablesListWaitComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'list-wait');
                    this.AvailablesItemsPerPageComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'items-per-page');
                    this.AvailablesPageComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'page');
                }
                {
                    var associatesGridComponent = UTIL.GetElByNameOnAnotherElement(null, _this.PrincipalComponent, 'associates-grid');
                    this.AssociatesRenderGridComponent = UTIL.GetElByNameOnAnotherElement(null, associatesGridComponent, 'grid-render-content');
                    this.AssociatesSearchButton = UTIL.GetElByNameOnAnotherElement(null, associatesGridComponent, 'executeButton');
                    this.AssociatesListWaitComponent = UTIL.GetElByNameOnAnotherElement(null, associatesGridComponent, 'list-wait');
                    this.AssociatesItemsPerPageComponent = UTIL.GetElByNameOnAnotherElement(null, associatesGridComponent, 'items-per-page');
                    this.AssociatesPageComponent = UTIL.GetElByNameOnAnotherElement(null, associatesGridComponent, 'page');
                }
            };
            RenderManyToManyAggregations.prototype.RenderHtmlBase = function () {
                var _this = this;
                var html = "\n<div style=\"padding:10px;\">\n    <div id='render-filters' style='float: left;'></div>\n    <div name=\"available-grid\" class=\"content-group available-grid no-expand\">\n        <div>\n            <div>\n                <div class=\"row\">\n                    <div class=\"form-group col-5\" style='margin-top: auto;text-align: right;'>\n                        <h4>" + _this.EntityMany.Plural + " " + Globalization_5.G('available', true) + "</h4>\n                    </div>\n                    <div class=\"form-group col-2\">\n                        <label for=\"filter-type\">" + Globalization_5.G('Page', false) + "</label>\n                        <select class=\"form-control\" name=\"page\">\n                                <option value=\"0\">1</option>\n                            </select>\n                    </div>\n\n                    <div class=\"form-group col-3\">\n                        <label for=\"filter-type\">" + Globalization_5.G('Items per page', true) + "</label>\n                        <select class=\"form-control\" name=\"items-per-page\">\n                                <option>5</option>\n                                <option>10</option>\n                                <option>20</option>\n                                <option>50</option>\n                                <option>100</option>\n                            </select>\n                    </div>\n\n                    <div class=\"form-group col-2\">\n                        <button type=\"button\" name=\"executeButton\" class=\"btn-list btn btn-primary submitButton\">" + Globalization_5.G('Search') + "</button>\n                        <img name=\"list-wait\" style=\"margin-top: 31px;\" src=\"" + CONSTANT_7.CONSTANT.URL_IMG_WAIT + "\" class=\"image-wait hide\" />\n                        <!--Todo - Arrumar esse endere\u00E7o acima-->\n                    </div>\n                </div>\n\n            </div>\n\n            <div>\n                <div style='overflow: auto;'>\n                    <div name=\"grid-render-content\" class=\"grid-render-content\"></div>\n                </div>\n            </div>\n        </div>\n    </div>\n    <!--second grid-->\n    <div class=\"associates-grid\" name=\"associates-grid\">\n        <div>\n            <div>\n                <div class=\"row\">\n                    <div class=\"col-12 associates-grid-content\">\n                        <div class=\"row\">\n\n                            <div class=\"form-group col-5\" style='margin-top: auto;text-align: right;'>\n                                <h4>" + _this.EntityMany.Plural + " " + Globalization_5.G('associates', true) + "</h4>\n                            </div>\n                            <div class=\"form-group col-2\">\n                                <label for=\"filter-type\">" + Globalization_5.G('Page', false) + "</label>\n                                <select class=\"form-control\" name=\"page\">\n                                        <option value=\"0\">1</option>\n                                </select>\n                            </div>\n\n                            <div class=\"form-group col-3\">\n                                <label for=\"filter-type\">" + Globalization_5.G('Items per page', true) + "</label>\n                                <select class=\"form-control\" name=\"items-per-page\">\n                                        <option>5</option>\n                                        <option>10</option>\n                                        <option>20</option>\n                                        <option>50</option>\n                                        <option>100</option>\n                                </select>\n                            </div>\n\n                            <div class=\"form-group col-2\">\n                                <button type=\"button\" name=\"executeButton\" class=\"btn-list btn btn-primary submitButton\">" + Globalization_5.G('Search') + "</button>\n                                <img name=\"list-wait\" style=\"margin-top: 31px;\" src=\"" + CONSTANT_7.CONSTANT.URL_IMG_WAIT + "\" class=\"image-wait hide\" />\n                                <!--Todo - Arrumar esse endere\u00E7o acima-->\n                            </div>\n\n                        </div>\n                    </div>\n                   \n                </div>\n\n            </div>\n\n            <div>\n                <div style='overflow: auto;'>\n                    <div name=\"grid-render-content\" class=\"grid-render-content\"></div>\n                </div>\n            </div>\n        </div>\n    </div>    \n</div>\n";
                var component = UTIL.CreateElementFromHTML(null, html);
                _this.RootElement.appendChild(component);
                _this.PrincipalComponent = component;
                var principalContentGroup = component.querySelector('[name="available-grid"]');
                var filterComponent = Render.RenderFilter(this.EntityMany.Type, 'render-filters', function (eventTyp, object) {
                    if (eventTyp == 'FormLoaded') {
                        _this.AvailablesSearchButton.dispatchEvent(new Event('click'));
                    }
                    else if (eventTyp == 'minimize') {
                        principalContentGroup.classList.add('expand');
                        principalContentGroup.classList.remove('no-expand');
                    }
                    else if (eventTyp == 'maximize') {
                        principalContentGroup.classList.add('no-expand');
                        principalContentGroup.classList.remove('expand');
                    }
                    else if (eventTyp == 'FlterChanged') {
                        _this.Filters = object;
                        _this.AvailablesSearchButton.dispatchEvent(new Event('click'));
                    }
                });
                filterComponent.StartFilter();
            };
            RenderManyToManyAggregations.prototype.StartEvents = function () {
                var _this = this;
                _this.AvailablesSearchButton.addEventListener('click', function (e) {
                    _this.AvailablesListWaitComponent.hidden = false;
                    _this.AvailablesSearchButton.hidden = true;
                    var filters = UTIL.Clone(_this.Filters);
                    for (var i = 0; i < filters.length; i++) {
                        var eleActual = filters[i];
                        var nextElement = filters[i + 1];
                        if (nextElement != null) {
                            eleActual.JunctionType = nextElement.JunctionType;
                        }
                    }
                    var jsonFilter = JSON.stringify(filters);
                    _this.LoadAvailableList(jsonFilter);
                });
            };
            RenderManyToManyAggregations.prototype.EntityToObject = function (data, entity) {
                var display = entity.Display;
                var localKeys = entity.ExternalKeys;
                var keyValues = entity.KeyValues;
                var text = entity.Text;
                var propertyName = entity.PropertyName;
                data[propertyName] = {};
                var obj = data[propertyName];
                obj[display] = text;
                for (var i = 0; i < localKeys.length; i++) {
                    obj[localKeys[i]] = keyValues[i];
                }
            };
            RenderManyToManyAggregations.prototype.OpenModal = function (type) {
                var _this = this;
                var root_id = _this.RootElement.parentElement.id;
                var render_ = Render.Render(type, root_id, true, true, null, function (message, modal_render) {
                    var dialog = modal_render.FormContainerComponent;
                    if (message == 'FormLoaded') {
                        dialog.classList.add('fluent-dialog');
                        var element = modal_render.FormComponent.querySelector("input, select, textarea");
                        element.focus();
                        modal_render.ShowDialog(modal_render, dialog, modal_render.JsonSchema.FluentJsonForm.Description);
                        var data = {};
                        _this.EntityToObject(data, _this.EntityOne);
                        _this.EntityToObject(data, _this.EntityMany);
                        render_.EditOrNew(render_, data);
                    }
                    else if (message == 'SavedSuccessfully') {
                        // const objectSaved = modal_render.ParameterToCallback[0][1] as DefaultResult;
                        // let obj = new Object();
                        // obj[element.PropertyName] = objectSaved.Data;
                        // SELECT2_RENDER.SetSelect2Value(render, obj, element.PropertyName, select);
                        dialog.hidden = true;
                        modal_render.Dispose(modal_render);
                    }
                });
                render_.Execute();
            };
            RenderManyToManyAggregations.prototype.LoadAvailableList = function (jsonFilter) {
                var _this = this;
                var itemsPerPage = this.AvailablesItemsPerPageComponent.value;
                var currentPage = this.AvailablesPageComponent.value;
                var header = [
                    { Key: 'itemsPerPage', Value: itemsPerPage },
                    { Key: 'currentPage', Value: currentPage },
                    { Key: 'startAtZero', Value: true },
                ];
                API.ListFilter.call(this, null, _this.EntityMany.Type, jsonFilter, header, this.RenderAvailableList);
            };
            RenderManyToManyAggregations.prototype.RenderAvailableList = function (data) {
                var _this = this;
                var pagination = data.Pagination;
                var obj = data.Data;
                var propertiesFiltered = _this.Schema.Properties.filter(function (x) { return x.GridTitle != null && x.GridTitle != ''; });
                var table = document.createElement('table');
                table.classList.add('agregations-table');
                var tHead = table.createTHead();
                var tBody = table.createTBody();
                var tHeadRow = tHead.insertRow();
                propertiesFiltered.forEach(function (x) {
                    var th = document.createElement('th');
                    tHeadRow.appendChild(th);
                    th.textContent = x.GridTitle;
                });
                obj.forEach(function (item) {
                    var row = tBody.insertRow();
                    propertiesFiltered.forEach(function (propertyName) {
                        var value = item[propertyName.PropertyName];
                        var cell = row.insertCell();
                        cell.innerText = value;
                    });
                    row.addEventListener('click', function (e) {
                        var keyProperties = _this.Schema.Properties.filter(function (x) { return x.IsKey; });
                        var keyNames = [];
                        var keyValues = [];
                        keyProperties.forEach(function (x) {
                            var keyName = x.PropertyName;
                            var keyValue = item[x.PropertyName];
                            keyNames.push(keyName);
                            keyValues.push(keyValue);
                        });
                        var query = UTIL.KeysToQueryString(keyNames, keyValues);
                        //const url = UTIL.FormatString(CONSTANT.URL_ANGULAR, type) + `${query}`;//Url for edit
                        _this.EntityMany.KeyNames = keyNames;
                        _this.EntityMany.KeyValues = keyValues;
                        _this.EntityMany.Text = item[_this.EntityMany.Display];
                        _this.OpenModal(_this.PrincipalType);
                    });
                });
                table.classList.add('table');
                _this.AvailableRenderGridComponent.innerHTML = '';
                _this.AvailableRenderGridComponent.appendChild(table);
                _this.AvailablesPageComponent.innerHTML = '';
                for (var i = 0; i < pagination.NumberOfPages; i++) {
                    var option = document.createElement('option');
                    option.text = (1 + i).toString();
                    option.value = i.toString();
                    if (pagination.CurrentPage == i) {
                        option.selected = true;
                    }
                    _this.AvailablesPageComponent.appendChild(option);
                }
                _this.AvailablesSearchButton.hidden = false;
                _this.AvailablesListWaitComponent.hidden = true;
            };
            RenderManyToManyAggregations.prototype.LoadAssociatesList = function (jsonFilter) {
                var _this = this;
                var itemsPerPage = this.AvailablesItemsPerPageComponent.value;
                var currentPage = this.AvailablesItemsPerPageComponent.value;
                var header = [
                    { Key: 'itemsPerPage', Value: itemsPerPage },
                    { Key: 'currentPage', Value: currentPage },
                    { Key: 'startAtZero', Value: true },
                ];
                API.ListFilter.call(this, null, _this.PrincipalType, jsonFilter, header, this.RenderAssociatesList);
            };
            RenderManyToManyAggregations.prototype.RenderAssociatesList = function (data) {
            };
            return RenderManyToManyAggregations;
        }());
        exports.RenderManyToManyAggregations = RenderManyToManyAggregations;
    });
    define("Aggregations/RenderManyToManyOptions", ["require", "exports", "API/API", "Util/Util", "Cache/Cache", "Render/Select2Render", "Util/Globalization", "Aggregations/RenderManyToManyAggregations", "Model/CONSTANT"], function (require, exports, API, UTIL, CACHE, SELECT2_RENDER, Globalization_6, RenderManyToManyAggregations_1, CONSTANT_8) {
        "use strict";
        exports.__esModule = true;
        var RenderManyToManyOptions = /** @class */ (function () {
            function RenderManyToManyOptions(form_name, root_element_id) {
                this.root_element_id = root_element_id;
                this.PrincipalType = form_name;
                this.RootElement = document.createElement('div');
                var root = document.getElementById(root_element_id); // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                if (root == null) {
                    throw root_element_id + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                if (this.RootElement == null) {
                    throw root_element_id + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                root.classList.add('container');
                root.appendChild(this.RootElement);
            }
            RenderManyToManyOptions.prototype.Start = function (userDefaultThema) {
                var _this_1 = this;
                if (userDefaultThema === void 0) { userDefaultThema = true; }
                var _this = this;
                UTIL.LoadInitialScripts(null, userDefaultThema, function () {
                    _this.GetJsonSchema(function (data) {
                        var properties = data.Properties.filter(function (property) {
                            var newProperty = property.FluentAggregation;
                            return newProperty != null && newProperty.IsManyToMany;
                        });
                        if (properties.length != 2) {
                            UTIL.Exception(null, "The number of entities 'FluentManyToManyAggregationAttribute' is different than expected. 2 entities are expected and we have " + properties.length);
                            return;
                        }
                        _this_1.ManyNoMany =
                            {
                                entityA: properties[0].FluentAggregation,
                                entityB: properties[1].FluentAggregation
                            };
                        _this_1.RenderFistOptions(_this);
                    });
                });
            };
            RenderManyToManyOptions.prototype.renderSelect2 = function () {
                var _this = this;
                var primaryOption = Globalization_6.G('Select') + " " + _this.EntityOne.Junction + " " + _this.EntityOne.Singular;
                var html = "\n<div name='select-a-element' class=\"div-content-group container-many-to-many-agregations-form\" name='container-many-to-many-agregations-form'>\n    <h4 class=\"title\">" + primaryOption + "</h4>\n    <div class=\"row\">\n        <div class=\"form-group col-12\">\n            <select class=\"form-control select2\" name=\"value-select2\">\n            </select>\n        </div>\n    </div>\n</div>\n";
                var select_a_element = this.RootElement.querySelector("[name='select-a-element']");
                if (select_a_element != null) {
                    select_a_element.remove();
                }
                var select2_a_element = this.RootElement.querySelector("[name='select2-element']");
                if (select2_a_element != null) {
                    select2_a_element.remove();
                }
                var component = UTIL.CreateElementFromHTML(null, html);
                this.RootElement.appendChild(component);
                var select = component.querySelector("select");
                SELECT2_RENDER.select2AutomaticList(null, select, _this.EntityOne);
                $(select).on('select2:select', function (e) {
                    _this.SelectId = e.params.data.id;
                    _this.SelectedName = e.params.data.text;
                    var entityOne = _this.EntityOne;
                    var manyEntity = _this.EntityMany;
                    entityOne.KeyValues = UTIL.StringToKeys(e.params.data.id);
                    entityOne.Text = _this.SelectedName;
                    _this.RootElement.remove();
                    var el = new RenderManyToManyAggregations_1.RenderManyToManyAggregations(_this.PrincipalType, entityOne, manyEntity, _this.root_element_id);
                    el.Start();
                });
                // _this.renderOperations();
            };
            //     renderOperations() {
            //         const _this = this;
            //         //const view = `${G('View')} ${_this.manyToManyEntitySelected.plural} ${G('from')} ${_this.manyToManyEntityNotSelected.junction} ${_this.manyToManyEntityNotSelected.singular}`;
            //         const update = `${G('Update')} ${_this.ManyToManyEntitySelected.Plural} ${G('from')} ${_this.ManyToManyEntityNotSelected.Junction} ${_this.ManyToManyEntityNotSelected.Singular}`;
            //         const add = `${G('Add')} ${_this.ManyToManyEntitySelected.Plural} ${G('to')} ${_this.ManyToManyEntityNotSelected.Junction} ${_this.ManyToManyEntityNotSelected.Singular}`;
            //         const remove = `${G('Remove')} ${_this.ManyToManyEntitySelected.Plural} ${G('from')} ${_this.ManyToManyEntityNotSelected.Junction} ${_this.ManyToManyEntityNotSelected.Singular}`;
            //         const label1 = G('What operation do you want to perform?');
            //         const html = `
            // <div name='select2-element' class="div-content-group container-many-to-many-agregations-form" name='container-many-to-many-agregations-form'>
            //     <h4 class="title">${label1}</h4>
            //     <div class="agregation-direction">
            //         <div name='update' class="agregation inactive ">
            //             <div>${update}</div>
            //         </div>
            //         <div name='add' class="agregation inactive">
            //             <div>${add}</div>
            //         </div>
            //         <div name='remove' class="agregation inactive">
            //             <div>${remove}</div>
            //         </div>
            //     </div>
            // </div>
            // `;
            //         const select_a_element = this.RootElement.querySelector("[name='select2-element']");
            //         if (select_a_element != null) { select_a_element.remove(); }
            //         const component = UTIL.CreateElementFromHTML(null, html);
            //         this.RootElement.appendChild(component);
            //         const updateComponent = UTIL.GetElByNameOnAnotherElement(null, component, 'update');
            //         const addComponent = UTIL.GetElByNameOnAnotherElement(null, component, 'add');
            //         const removeComponent = UTIL.GetElByNameOnAnotherElement(null, component, 'remove');
            //         updateComponent.addEventListener('click', function () {
            //             // _this.Url += `&op=u`;
            //             // var win = window.open(_this.Url, '_blank');
            //             // win.focus();
            //             Finish();
            //         });
            //         addComponent.addEventListener('click', function () {
            //             // _this.Url += `&op=a`;
            //             // var win = window.open(_this.Url, '_blank');
            //             // win.focus();
            //             Finish();
            //         });
            //         removeComponent.addEventListener('click', function () {
            //             _this.Url += `&op=r`;
            //             // var win = window.open(_this.Url, '_blank');
            //             // win.focus();
            //             Finish();
            //         });
            //         function Finish() {
            //             _this.RootElement.innerHTML = '';
            //         }
            //     }
            RenderManyToManyOptions.prototype.GetJsonSchema = function (callback) {
                var _this = this;
                var url = UTIL.FormatString(CONSTANT_8.CONSTANT.URL_JSONFORM, _this.PrincipalType, 'false');
                var schema = CACHE.Get(url);
                if (schema != null) {
                    callback(schema);
                    return;
                }
                API.GetDataFromServer(null, url, function (data) {
                    CACHE.Set(url, data);
                    callback(data);
                });
            };
            RenderManyToManyOptions.prototype.RenderFistOptions = function (_this) {
                var label1 = Globalization_6.G("What do you want to manipulate?");
                var primaryOption = this.ManyNoMany.entityA.Plural + " " + Globalization_6.G('for') + " " + this.ManyNoMany.entityB.Junction + " " + this.ManyNoMany.entityB.Singular;
                var secundaryOption = this.ManyNoMany.entityB.Plural + " " + Globalization_6.G('for') + " " + this.ManyNoMany.entityA.Junction + " " + this.ManyNoMany.entityA.Singular;
                {
                    var html = "\n<div class=\"div-content-group container-many-to-many-agregations-form\" name='container-many-to-many-agregations-form'>\n    <h4 class=\"title\">" + label1 + "</h4>\n    <div class=\"agregation-direction\">\n        <div name='option-a' class=\"agregation inactive\">\n            <div>" + primaryOption + "</div>\n        </div>\n\n        <div name='option-b' class=\"agregation inactive\">\n            <div>" + secundaryOption + "</div>\n        </div>\n    </div>\n</div>\n";
                    var optionComponent = UTIL.CreateElementFromHTML(null, html);
                    this.RootElement.appendChild(optionComponent);
                    var option_a_1 = UTIL.GetElByNameOnAnotherElement(null, optionComponent, 'option-a');
                    var option_b_1 = UTIL.GetElByNameOnAnotherElement(null, optionComponent, 'option-b');
                    option_a_1.addEventListener('click', function (e) {
                        option_a_1.classList.remove('inactive');
                        option_a_1.classList.add('active');
                        option_b_1.classList.remove('active');
                        option_b_1.classList.add('inactive');
                        _this.EntityOne = _this.ManyNoMany.entityB;
                        _this.EntityMany = _this.ManyNoMany.entityA;
                        _this.renderSelect2();
                    });
                    option_b_1.addEventListener('click', function (e) {
                        option_b_1.classList.remove('inactive');
                        option_b_1.classList.add('active');
                        option_a_1.classList.remove('active');
                        option_a_1.classList.add('inactive');
                        _this.EntityOne = _this.ManyNoMany.entityA;
                        _this.EntityMany = _this.ManyNoMany.entityB;
                        _this.renderSelect2();
                    });
                }
            };
            return RenderManyToManyOptions;
        }());
        exports.RenderManyToManyOptions = RenderManyToManyOptions;
    });
    define("List/RenderListClass", ["require", "exports", "API/API", "Util/Util", "Util/Globalization", "Model/CONSTANT"], function (require, exports, API, UTIL, Globalization_7, CONSTANT_9) {
        "use strict";
        exports.__esModule = true;
        var RenderListClass = /** @class */ (function () {
            function RenderListClass(parameters) {
                this.Filters = [];
                CONSTANT_9.CONSTANT.SetParameters(parameters);
                this.parameters = parameters;
                this.root_element_id = parameters.RootElementId;
                this.PrincipalType = parameters.FormName;
                this.RootElement = document.createElement('div');
                var root = document.getElementById(parameters.RootElementId); // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                if (root == null) {
                    throw parameters.RootElementId + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                if (this.RootElement == null) {
                    throw parameters.RootElementId + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                root.classList.remove('container');
                root.appendChild(this.RootElement);
            }
            RenderListClass.prototype.Start = function (userDefaultThema) {
                if (userDefaultThema === void 0) { userDefaultThema = true; }
                var _this = this;
                UTIL.LoadInitialScripts(null, userDefaultThema, function () {
                    API.GetJsonSchema(null, _this.PrincipalType, function (data) {
                        _this.Schema = data;
                        _this.RegisterComponents();
                        _this.StartEvents.call(_this);
                    });
                });
            };
            RenderListClass.prototype.RegisterComponents = function () {
                var _this = this;
                this.RenderHtmlBase();
                {
                    var availableGridComponent = UTIL.GetElByNameOnAnotherElement(null, _this.PrincipalComponent, 'available-grid');
                    this.AvailableRenderGridComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'grid-render-content');
                    this.AvailablesSearchButton = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'executeButton');
                    this.AvailablesListWaitComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'list-wait');
                    this.AvailablesItemsPerPageComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'items-per-page');
                    this.AvailablesPageComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'page');
                    this.TotalItemsComponent = UTIL.GetElByNameOnAnotherElement(null, availableGridComponent, 'TotalItems');
                }
                // {
                //     const associatesGridComponent = UTIL.GetElByNameOnAnotherElement(null,
                //         _this.PrincipalComponent, 'associates-grid') as HTMLElement;
                //     this.AssociatesRenderGridComponent = UTIL.GetElByNameOnAnotherElement(null,
                //         associatesGridComponent, 'grid-render-content') as HTMLElement;
                //     this.AssociatesSearchButton = UTIL.GetElByNameOnAnotherElement(null,
                //         associatesGridComponent, 'executeButton') as HTMLButtonElement;
                //     this.AssociatesListWaitComponent = UTIL.GetElByNameOnAnotherElement(null,
                //         associatesGridComponent, 'list-wait') as HTMLElement;
                //     this.AssociatesItemsPerPageComponent = UTIL.GetElByNameOnAnotherElement(null,
                //         associatesGridComponent, 'items-per-page') as HTMLSelectElement;
                //     this.AssociatesPageComponent = UTIL.GetElByNameOnAnotherElement(null,
                //         associatesGridComponent, 'page') as HTMLSelectElement;
                // }
            };
            RenderListClass.prototype.RenderHtmlBase = function () {
                var _this = this;
                var html = "\n<div style=\"padding:10px;\">\n    <div id='render-filters' style='float: left;'></div>\n    <div name=\"available-grid\" class=\"content-group available-grid no-expand\">\n        <div>\n            <div>\n                <div class=\"row\">\n                    <div class=\"form-group col-4\" style='margin-top: auto;'>\n                        <h4>" + _this.Schema.FluentJsonForm.Name + " " + Globalization_7.G('available', true) + " <span name='TotalItems'></span></h4> \n                    </div>\n                    <div class=\"form-group col-2\">\n                        <label for=\"filter-type\">" + Globalization_7.G('Page', false) + "</label>\n                        <select class=\"form-control\" name=\"page\">\n                                <option value=\"0\">1</option>\n                        </select>\n                    </div>\n\n                    <div class=\"form-group col-3\">\n                        <label for=\"filter-type\">" + Globalization_7.G('Items per page', true) + "</label>\n                        <select class=\"form-control\" name=\"items-per-page\">\n                                <option>5</option>\n                                <option>10</option>\n                                <option>20</option>\n                                <option>50</option>\n                                <option>100</option>\n                        </select>\n                    </div>\n\n                    <div class=\"form-group col-1\" style='padding-top: 37px;'>\n                        <a href='" + CONSTANT_9.CONSTANT.BASE_CLIENT_URL + "?file=/html/index.html&form=" + _this.PrincipalType + "'>" + Globalization_7.G('New') + "</a>\n                    </div>\n\n                    <div class=\"form-group col-1\">\n                        <button type=\"button\" name=\"executeButton\" class=\"btn-list btn btn-primary submitButton\">" + Globalization_7.G('Search') + "</button>\n                        <img name=\"list-wait\" class=\"image-wait\" hidden style=\"height: 40px;margin-top: 31px;\" src=\"" + CONSTANT_9.CONSTANT.URL_IMG_WAIT + "\" />\n                    </div>\n                </div>\n\n            </div>\n\n            <div>\n                <div style='overflow: auto;'>\n                    <div name=\"grid-render-content\" class=\"grid-render-content\"></div>\n                </div>\n            </div>\n        </div>\n    </div> \n</div>\n";
                var component = UTIL.CreateElementFromHTML(null, html);
                _this.RootElement.appendChild(component);
                _this.PrincipalComponent = component;
                var principalContentGroup = component.querySelector('[name="available-grid"]');
                var filterComponent = Render.RenderFilter(this.PrincipalType, 'render-filters', function (eventTyp, object) {
                    if (eventTyp == 'FormLoaded') {
                        _this.AvailablesSearchButton.dispatchEvent(new Event('click'));
                    }
                    else if (eventTyp == 'minimize') {
                        principalContentGroup.classList.add('expand');
                        principalContentGroup.classList.remove('no-expand');
                    }
                    else if (eventTyp == 'maximize') {
                        principalContentGroup.classList.add('no-expand');
                        principalContentGroup.classList.remove('expand');
                    }
                    else if (eventTyp == 'FlterChanged') {
                        _this.Filters = object;
                        _this.AvailablesSearchButton.dispatchEvent(new Event('click'));
                    }
                });
                filterComponent.StartFilter();
            };
            RenderListClass.prototype.StartEvents = function () {
                var _this = this;
                _this.AvailablesSearchButton.addEventListener('click', function (e) {
                    _this.AvailablesListWaitComponent.hidden = false;
                    _this.AvailablesSearchButton.hidden = true;
                    var filters = UTIL.Clone(_this.Filters);
                    for (var i = 0; i < filters.length; i++) {
                        var eleActual = filters[i];
                        var nextElement = filters[i + 1];
                        if (nextElement != null) {
                            eleActual.JunctionType = nextElement.JunctionType;
                        }
                    }
                    var jsonFilter = JSON.stringify(filters);
                    _this.LoadAvailableList(jsonFilter);
                });
            };
            RenderListClass.prototype.LoadAvailableList = function (jsonFilter) {
                var _this = this;
                var itemsPerPage = this.AvailablesItemsPerPageComponent.value;
                var currentPage = this.AvailablesPageComponent.value;
                var header = [
                    { Key: 'itemsPerPage', Value: itemsPerPage },
                    { Key: 'currentPage', Value: currentPage },
                    { Key: 'startAtZero', Value: true },
                ];
                API.ListFilter.call(this, null, _this.PrincipalType, jsonFilter, header, this.RenderAvailableList);
            };
            RenderListClass.prototype.RenderAvailableList = function (data) {
                var _this = this;
                var pagination = data.Pagination;
                var obj = data.Data;
                var propertiesFiltered = _this.Schema.Properties.filter(function (x) { return x.GridTitle != null && x.GridTitle != ''; });
                var table = document.createElement('table');
                table.classList.add('agregations-table');
                var tHead = table.createTHead();
                var tBody = table.createTBody();
                var tHeadRow = tHead.insertRow();
                propertiesFiltered.forEach(function (x) {
                    var th = document.createElement('th');
                    tHeadRow.appendChild(th);
                    th.textContent = x.GridTitle;
                });
                obj.forEach(function (item) {
                    var row = tBody.insertRow();
                    propertiesFiltered.forEach(function (propertyName) {
                        var value = item[propertyName.PropertyName];
                        var cell = row.insertCell();
                        cell.innerText = value;
                    });
                    row.addEventListener('click', function (e) {
                        var keyProperties = _this.Schema.Properties.filter(function (x) { return x.IsKey; });
                        var keyNames = [];
                        var keyValues = [];
                        keyProperties.forEach(function (x) {
                            var keyName = x.PropertyName;
                            var keyValue = item[x.PropertyName];
                            keyNames.push(keyName);
                            keyValues.push(keyValue);
                        });
                        var query = UTIL.KeysToQueryString(keyNames, keyValues);
                        var url = UTIL.FormatString(CONSTANT_9.CONSTANT.URL_ANGULAR, _this.PrincipalType) + ("" + query); //Url for edit
                        window.location.href = url;
                        //window.open(url);
                        // _this.EntityMany.KeyNames = keyNames;
                        // _this.EntityMany.KeyValues = keyValues;
                        // _this.EntityMany.Text = item[_this.EntityMany.Display];
                        // _this.OpenModal(_this.PrincipalType);
                    });
                });
                table.classList.add('table');
                _this.AvailableRenderGridComponent.innerHTML = '';
                _this.AvailableRenderGridComponent.appendChild(table);
                _this.TotalItemsComponent.innerHTML = pagination.TotalQuantityOfItems.toString();
                _this.AvailablesPageComponent.innerHTML = '';
                for (var i = 0; i < pagination.NumberOfPages; i++) {
                    var option = document.createElement('option');
                    option.text = (1 + i).toString();
                    option.value = i.toString();
                    if (pagination.CurrentPage == i) {
                        option.selected = true;
                    }
                    _this.AvailablesPageComponent.appendChild(option);
                }
                _this.AvailablesSearchButton.hidden = false;
                _this.AvailablesListWaitComponent.hidden = true;
            };
            RenderListClass.prototype.LoadAssociatesList = function (jsonFilter) {
                var _this = this;
                var itemsPerPage = this.AvailablesItemsPerPageComponent.value;
                var currentPage = this.AvailablesItemsPerPageComponent.value;
                var header = [
                    { Key: 'itemsPerPage', Value: itemsPerPage },
                    { Key: 'currentPage', Value: currentPage },
                    { Key: 'startAtZero', Value: true },
                ];
                API.ListFilter.call(this, null, _this.PrincipalType, jsonFilter, header, this.RenderAssociatesList);
            };
            RenderListClass.prototype.RenderAssociatesList = function (data) {
            };
            return RenderListClass;
        }());
        exports.RenderListClass = RenderListClass;
    });
    define("render", ["require", "exports", "Util/Util", "API/API", "Cache/Cache", "Render/FormRender", "Enum/EnumForm", "Render/Select2Render", "Tests/RenderTestClass", "Filter/RenderFilterClass", "Aggregations/RenderManyToManyOptions", "Util/Globalization", "Services/MessageServiceClass", "List/RenderListClass", "Model/CONSTANT"], function (require, exports, UTIL, API, CACHE, FORM_RENDER, EnumForm_4, SELECT2_RENDER, RenderTestClass_1, RenderFilterClass_1, RenderManyToManyOptions_1, Globalization_8, MessageServiceClass_1, RenderListClass_1, CONSTANT_10) {
        "use strict";
        exports.__esModule = true;
        function Render(parameters) {
            return new RenderClass(parameters);
        }
        exports.Render = Render;
        function RenderTest(parameters) {
            return new RenderTestClass_1.RenderTestClass(parameters);
        }
        exports.RenderTest = RenderTest;
        function RenderFilter(form_name, root_element_id, CallBack) {
            return new RenderFilterClass_1.RenderFilterClass(form_name, root_element_id, CallBack);
        }
        exports.RenderFilter = RenderFilter;
        function RenderManyToManyAggregations(form_name, root_element_id) {
            return new RenderManyToManyOptions_1.RenderManyToManyOptions(form_name, root_element_id);
        }
        exports.RenderManyToManyAggregations = RenderManyToManyAggregations;
        function RenderList(parameters) {
            return new RenderListClass_1.RenderListClass(parameters);
        }
        exports.RenderList = RenderList;
        var RenderClass = /** @class */ (function () {
            function RenderClass(parameters) {
                this.IsEdit = false;
                if (CONSTANT_10.CONSTANT.USE_CACHE == false) {
                    console.warn('Attention! Cache control is disabled by the application.');
                }
                CONSTANT_10.CONSTANT.SetParameters(parameters);
                this.Parameters = parameters;
                this.principalType = parameters.FormName;
                this.OperationsCallback = parameters.Callback;
                this.Tablet = parameters.IsTablet;
                this.IsPrincipal = parameters.IsPrincipal;
                this.RootElement = document.createElement('div');
                var root = document.getElementById(parameters.RootElementId); // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                if (root == null) {
                    throw parameters.RootElementId + " not found"; // Esse não pode chamar o utilitário, pois não ha nada na tela ainda
                }
                if (parameters.IsPrincipal) {
                    this.Template = document.createElement('div');
                    root.appendChild(this.Template);
                }
                else {
                    if (parameters.Template == null) {
                        throw "A template must be entered.";
                    }
                    this.Template = parameters.Template;
                }
                root.appendChild(this.RootElement);
            }
            RenderClass.prototype.Execute = function (userDefaultThema) {
                if (userDefaultThema === void 0) { userDefaultThema = true; }
                this.InitializeOperations(this, userDefaultThema);
            };
            RenderClass.prototype.IsEditCheck = function (render) {
                if (!render.IsPrincipal) {
                    return;
                }
                var parameters = UTIL.GetParams();
                var keys = $.map(parameters.filter(function (x) { return x.Key == 'kn'; }), function (item) { return item.Values; });
                if (keys.length > 0) {
                    this.IsEdit = true;
                }
            };
            RenderClass.prototype.InitializeOperations = function (render, userDefaultThema) {
                UTIL.LoadInitialScripts(render, userDefaultThema, function () {
                    render.CheckCacheVersions(render, function () {
                        render.GetJsonSchema(render, function (data) {
                            render.IsEditCheck(render);
                            render.JsonSchema = data;
                            render.AddFormToDOM(render, data);
                            FORM_RENDER.RenderForm(render, data, render.FormComponent, false);
                            render.FormLoaded(render);
                        });
                    });
                });
            };
            RenderClass.prototype.CheckCacheVersions = function (render, callback) {
                var clearCache = UTIL.GetParameterByName('clear-cache');
                if (clearCache != null) {
                    CACHE.Clear();
                    render.IsPrincipal == true;
                }
                if (render.IsPrincipal == false) {
                    callback();
                    return;
                }
                function LoadTemplateForced(callback) {
                    var templateUrl = UTIL.FormatString(CONSTANT_10.CONSTANT.URL_TEMPLATE, render.principalType);
                    API.GetDataFromServer(render, templateUrl, function (html) {
                        CACHE.Set(templateUrl, html);
                        callback(html);
                    });
                }
                var url = UTIL.FormatString(CONSTANT_10.CONSTANT.URL_VERSION);
                var templatesCacheVersion = CACHE.Get(url + 'template');
                if (templatesCacheVersion == null) {
                    LoadTemplateForced(function (html) {
                        render.Template.innerHTML = html;
                        callback();
                    });
                }
                else {
                    var templateUrl = UTIL.FormatString(CONSTANT_10.CONSTANT.URL_TEMPLATE, render.principalType);
                    var html = CACHE.Get(templateUrl);
                    render.Template.innerHTML = html;
                    callback();
                }
                API.GetDataFromServer(render, url, function (versions) {
                    var schamasCacheVersion = CACHE.Get(url + 'schema');
                    if (schamasCacheVersion == null || versions.schema != schamasCacheVersion) {
                        if (schamasCacheVersion != null) {
                            CACHE.Clear();
                            templatesCacheVersion = 'diff';
                        }
                        CACHE.Set(url + 'schema', versions.schema);
                    }
                    if (versions.template != templatesCacheVersion) {
                        if (templatesCacheVersion != null) {
                            LoadTemplateForced(function () { }); //Call not blocking
                        }
                        CACHE.Set(url + 'template', versions.template);
                    }
                });
            };
            RenderClass.prototype.GetJsonSchema = function (render, callback) {
                var url = UTIL.FormatString(CONSTANT_10.CONSTANT.URL_JSONFORM, render.principalType, render.Tablet.toString());
                var schema = CACHE.Get(url);
                if (schema != null) {
                    callback(schema);
                    return;
                }
                API.GetDataFromServer(render, url, function (data) {
                    CACHE.Set(url, data);
                    callback(data);
                });
            };
            RenderClass.prototype.AddFormToDOM = function (render, data) {
                var keyValueDictionary = [];
                keyValueDictionary.push({ Key: 'Submit', Value: Globalization_8.G('Submit') });
                keyValueDictionary.push({ Key: 'Remove', Value: Globalization_8.G('Remove') });
                keyValueDictionary.push({ Key: 'Clone', Value: Globalization_8.G('Clone') });
                keyValueDictionary.push({ Key: 'LoadingData', Value: Globalization_8.G('Loadin data...') });
                keyValueDictionary.push({ Key: 'IconAdd', Value: CONSTANT_10.CONSTANT.URL_IMG_ADD });
                keyValueDictionary.push({ Key: 'IconWait', Value: CONSTANT_10.CONSTANT.URL_IMG_WAIT });
                var form = UTIL.getTemplate(render, EnumForm_4.EnumForm.FORM);
                var containerForm = UTIL.VariableTransformer(render, form.innerHTML, data.FluentJsonForm, keyValueDictionary);
                render.RootElement.append(containerForm);
                this.MessageService = new MessageServiceClass_1.MessageServiceClass(containerForm);
                render.InitializeComponents(render, containerForm);
                render.AttachhEvents(render);
            };
            RenderClass.prototype.InitializeComponents = function (render, containerForm) {
                render.FormContainerComponent = containerForm;
                render.FormComponent = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, 'principal-form');
                render.FormDivContainerLoadContent = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, 'form-div-container-load-content');
                render.FormLoading = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, 'loading-form');
                render.InteligentReferenceList = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, "inteligent-reference-list");
                render.WaitImageSubmit = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, 'submit-wait');
                render.SubmitButton = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, "submitButton");
                render.RemoveButton = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, "removeButton");
                render.CloneButton = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, "cloneButton");
                render.ModalTitle = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, "modal-title");
                render.DialogClose = UTIL.GetElByNameOnAnotherElement(render, render.FormContainerComponent, "fluent-dialog-close");
            };
            RenderClass.prototype.AttachhEvents = function (render) {
                render.SubmitButton.addEventListener("click", function () {
                    render.SubmitForm(render, render.principalType);
                });
                render.RemoveButton.addEventListener("click", function () {
                    render.RemoveForm(render, render.principalType);
                });
                render.CloneButton.addEventListener("click", function () {
                    // render.CloneForm(render, render.principalType);
                });
            };
            RenderClass.prototype.FormLoaded = function (render) {
                render.OperationsCallback('FormLoaded', render);
                if (render.IsEdit) {
                    render.LoadDataForEdit(render);
                }
            };
            RenderClass.prototype.LoadDataForEdit = function (render) {
                var parameters = UTIL.GetParams();
                var keys = $.map(parameters.filter(function (x) { return x.Key == 'kn'; }), function (item) { return item.Values; });
                var values = $.map(parameters.filter(function (x) { return x.Key == 'kv'; }), function (item) { return item.Values; });
                if (keys.length == 0) {
                    return;
                }
                render.LoadDataByKey(render, keys, values);
            };
            RenderClass.prototype.EditByKey = function (render, keyNames, keyValues) {
                render.IsEdit = true;
                render.LoadDataByKey(render, keyNames, keyValues);
            };
            RenderClass.prototype.Edit = function (render, data) {
                render.IsEdit = true;
                render.LoadDataForEditByData(render, data);
            };
            RenderClass.prototype.EditOrNew = function (render, data) {
                render.IsEditOrNew = true;
                render.LoadDataForEditByData(render, data);
            };
            RenderClass.prototype.LoadDataByKey = function (render, keyNames, keyValues) {
                render.ShowLoadingEdit(render);
                // render: IRenderClass,
                // type: string,
                // jsonFilter: string,
                // header: KeyValue[],
                // callback: (data: DefaultResult) => void) {
                var filters = UTIL.KeysToFilters(render, keyNames, keyValues);
                API.FindByFilter(render, render.principalType, filters, null, function (data) {
                    render.LoadDataForEditByData(render, data.Data);
                    render.IsEdit = true;
                    render.FormComponent.hidden = false;
                    render.RemoveButton.hidden = false;
                    render.HiddenLoadingEdit(render);
                });
            };
            RenderClass.prototype.ShowLoadingEdit = function (render) {
                render.FormDivContainerLoadContent.hidden = true;
                render.FormLoading.hidden = false;
            };
            RenderClass.prototype.HiddenLoadingEdit = function (render) {
                render.FormDivContainerLoadContent.hidden = false;
                render.FormLoading.hidden = true;
                SELECT2_RENDER.ReloadSelect2(render.FormComponent);
            };
            RenderClass.prototype.LoadDataForEditByData = function (render, data) {
                var schema = render.JsonSchema;
                render.RecursiveSetEditValueForm(render, schema, data, null);
                // schema.Properties.forEach(property => {
                //     if (property.IsList) {
                //         return;
                //     }
                //     const value = data[property.PropertyName];
                //     if (value == null) {
                //         return;
                //     }
                //     const element = UTIL.GetElByNameOnAnotherElement(render, render.FormComponent, property.PropertyName) as HTMLInputElement;
                //     if (property.Form.toString() == EnumForm[EnumForm.DATEPICKER]) {
                //         const date = new Date(value)
                //         element.value = date.toISOString().substring(0, 10);
                //     }
                //     else if (element.classList.contains('select2')) {
                //         const isComposition: boolean = false;
                //         const compositionPropertyName: string = '';
                //         SELECT2_RENDER.SetSelect2Value(render, render.FormComponent, data, property.PropertyName, element, isComposition, compositionPropertyName);
                //     }
                //     else {
                //         element.value = value;
                //     }
                // });
            };
            RenderClass.prototype.RecursiveSetEditValueForm = function (render, schema, data, compositionPropertyName) {
                schema.Properties.forEach(function (property) {
                    //Check is complex object
                    if (property.IsList) {
                        if (property.FluentComposition != null) {
                            var list = data[property.FluentComposition.PropertyName];
                            var i_1 = 0;
                            list.forEach(function (item) {
                                var localCompositionName = property.FluentComposition.PropertyName + "[" + i_1 + "]";
                                localCompositionName = compositionPropertyName == null ? localCompositionName : compositionPropertyName + "." + localCompositionName;
                                render.RecursiveSetEditValueForm(render, property.FluentComposition.Form, item, localCompositionName);
                            });
                        }
                        return;
                    }
                    var fieldName = compositionPropertyName == null ? property.PropertyName : compositionPropertyName + "." + property.PropertyName;
                    var value = data[property.PropertyName];
                    if (value == null) {
                        return;
                    }
                    var element = UTIL.GetElByNameOnAnotherElement(render, render.FormComponent, fieldName);
                    if (property.Form.toString() == EnumForm_4.EnumForm[EnumForm_4.EnumForm.DATEPICKER]) {
                        var date = new Date(value);
                        element.value = date.toISOString().substring(0, 10);
                    }
                    else if (element.classList.contains('select2')) {
                        var isComposition = compositionPropertyName != null;
                        SELECT2_RENDER.SetSelect2Value(render, schema, render.FormComponent, data, property.PropertyName, element, isComposition, compositionPropertyName);
                    }
                    else {
                        element.value = value;
                    }
                });
            };
            RenderClass.prototype.SucessfulSave = function (render, objectSaved) {
                render.ParameterToCallback = [['objectSaved', objectSaved]];
                UTIL.Sucess(render, Globalization_8.G('Saved Successfully'));
                render.OperationsCallback('SavedSuccessfully', render);
            };
            RenderClass.prototype.SucessfulRemove = function (render, objectRemoved) {
                render.ParameterToCallback = [['objectRemoved', objectRemoved]];
                UTIL.Sucess(render, Globalization_8.G('Removed Successfully'));
                render.OperationsCallback('RemovedSuccessfully', render);
            };
            RenderClass.prototype.ShowHideWaitOperations = function (render, show) {
                if (show) {
                    render.WaitImageSubmit.hidden = false;
                    render.CloneButton.hidden = true;
                    render.SubmitButton.hidden = true;
                    render.RemoveButton.hidden = true;
                }
                else {
                    render.WaitImageSubmit.hidden = true;
                    render.CloneButton.hidden = false;
                    render.SubmitButton.hidden = false;
                    if (render.IsEdit && render.IsPrincipal) {
                        render.RemoveButton.hidden = false;
                    }
                }
            };
            RenderClass.prototype.CloneForm = function (render, type) {
            };
            RenderClass.prototype.RemoveForm = function (render, type) {
                var formElement = render.FormComponent;
                render.MessageService.Clear();
                render.ShowHideWaitOperations(render, true);
                var json = UTIL.FormToJSONString(render, formElement);
                API.Remove(render, type, json, function (render, status, responseText) {
                    render.ShowHideWaitOperations(render, false);
                    if (status == 200) {
                        var objectSaved = JSON.parse(responseText);
                        render.SucessfulRemove(render, objectSaved);
                    }
                    else {
                        render.ShowInconsistencesForm(render, status, responseText);
                    }
                });
            };
            RenderClass.prototype.SubmitForm = function (render, type) {
                var formElement = render.FormComponent;
                render.MessageService.Clear();
                render.ShowHideWaitOperations(render, true);
                var json = UTIL.FormToJSONString(render, formElement);
                API.AddOrUpdate(render, type, json, function (render, status, responseText) {
                    render.ShowHideWaitOperations(render, false);
                    if (status == 200) {
                        var objectSaved = JSON.parse(responseText);
                        render.SucessfulSave(render, objectSaved);
                    }
                    else {
                        render.ShowInconsistencesForm(render, status, responseText);
                    }
                });
            };
            RenderClass.prototype.ShowInconsistencesForm = function (render, status, responseText) {
                if (UTIL.IsJsonString(responseText)) {
                    var error = JSON.parse(responseText);
                    if (error.ValidationError) {
                        render.ShowInconsistences(render, render.FormComponent, error);
                        render.OperationsCallback('SaveError-Inconsistence', render);
                        return;
                    }
                }
                UTIL.Exception(render, responseText, status);
            };
            RenderClass.prototype.ShowInconsistences = function (render, parentHtmlElement, error) {
                var first = false;
                error.Inconsistencies.forEach(function (inconsistence) {
                    var field = inconsistence.Field;
                    var PropertyName = inconsistence.PropertyName;
                    if (PropertyName == null) {
                        UTIL.Exception(render, inconsistence.Message);
                        return;
                    }
                    var element = UTIL.GetElByNameOnAnotherElement(render, parentHtmlElement, PropertyName);
                    var header = element;
                    if (header != null && header.type == 'hidden') {
                        var agregations = render.JsonSchema.Properties.filter(function (x) {
                            return x.FluentAggregation != null &&
                                x.FluentAggregation.LocalKeys.filter(function (y) { return y == PropertyName; }).length > 0;
                        });
                        if (agregations != null && agregations.length > 0) {
                            PropertyName = agregations[0].PropertyName;
                            element = UTIL.GetElByNameOnAnotherElement(render, parentHtmlElement, PropertyName);
                            //element.parentElement.classList.add('is-invalid-field');
                        }
                    }
                    element.closest('.form-group').classList.add('is-invalid-field');
                    // element.classList.add('is-invalid-field');
                    if (!first) {
                        first = true;
                        element.focus();
                    }
                    var elementError = UTIL.GetElByNameOnAnotherElement(render, parentHtmlElement, "error-" + PropertyName);
                    if (elementError) {
                        elementError.textContent = inconsistence.Message;
                    }
                });
            };
            RenderClass.prototype.StartCacheAggregationLoad = function (render, agregations) {
                agregations.forEach(function (element) {
                    var type = element.FluentAggregation.Type;
                    var url = UTIL.FormatString(CONSTANT_10.CONSTANT.URL_JSONFORM, type, true.toString());
                    var schema = CACHE.Get(url);
                    if (schema != null) {
                        return;
                    }
                    API.GetDataFromServer(render, url, function (data) {
                        CACHE.Set(url, data);
                    });
                });
            };
            RenderClass.prototype.LoadAggregations = function (render, parentHtmlElement, form, isComposition, compositionPropertyName) {
                var agregations = form.Properties.filter(function (x) { return x.FluentAggregation != null; });
                agregations.forEach(function (element) {
                    var fieldName = element.PropertyName;
                    if (isComposition) {
                        fieldName = compositionPropertyName + "." + element.PropertyName;
                    }
                    var select = UTIL.GetElByNameOnAnotherElement(render, parentHtmlElement, fieldName);
                    render.AddReferenceLink(render, parentHtmlElement, element, select, isComposition, compositionPropertyName);
                    if (!isComposition) {
                        render.AddInteligentReferenceLinks(render, element);
                    }
                    SELECT2_RENDER.select2AutomaticList(render, select, element.FluentAggregation);
                    SELECT2_RENDER.select2Event(render, parentHtmlElement, select, element, isComposition, compositionPropertyName);
                });
                render.StartCacheAggregationLoad(render, agregations);
            };
            RenderClass.prototype.AddReferenceLink = function (render, parentHtmlElement, element, select, isComposition, compositionPropertyName) {
                var type = element.FluentAggregation.Type;
                var name = "reference-" + element.PropertyName;
                if (compositionPropertyName != null) {
                    name = "reference-" + compositionPropertyName + "." + element.PropertyName;
                }
                var reference = UTIL.GetElByNameOnAnotherElement(render, parentHtmlElement, name);
                reference.hidden = false;
                reference.addEventListener('click', function (e) {
                    var root_id = render.RootElement.parentElement.id;
                    var parameters = UTIL.Clone(render.Parameters);
                    parameters.FormName = type;
                    parameters.RootElementId = root_id;
                    parameters.IsTablet = true;
                    parameters.IsPrincipal = false;
                    parameters.Template = render.Template;
                    parameters.Callback = Callback;
                    Render(parameters).Execute();
                    function Callback(message, modal_render) {
                        var dialog = modal_render.FormContainerComponent;
                        if (message == 'FormLoaded') {
                            dialog.classList.add('fluent-dialog');
                            var element_1 = modal_render.FormComponent.querySelector("input, select, textarea");
                            element_1.focus();
                            modal_render.ShowDialog(modal_render, dialog, modal_render.JsonSchema.FluentJsonForm.Description);
                        }
                        else if (message == 'SavedSuccessfully') {
                            var objectSaved = modal_render.ParameterToCallback[0][1];
                            var obj = new Object();
                            obj[element.PropertyName] = objectSaved.Data;
                            SELECT2_RENDER.SetSelect2Value(render, render.JsonSchema, parentHtmlElement, obj, element.PropertyName, select, isComposition, compositionPropertyName);
                            dialog.hidden = true;
                            modal_render.Dispose(modal_render);
                        }
                    }
                });
            };
            RenderClass.prototype.ShowDialog = function (render_modal, dialog, title) {
                dialog.style.left = ((window.innerWidth - dialog.clientWidth) / 2) + 'px';
                // dialog.style.top = '50px';
                render_modal.DialogClose.addEventListener('click', function (e) {
                    dialog.hidden = true;
                    render_modal.Dispose(render_modal);
                });
                render_modal.ModalTitle.innerHTML = title;
                dialog.hidden = false;
            };
            RenderClass.prototype.AddInteligentReferenceLinks = function (render, element) {
                var inteligentReference = document.createElement('a');
                inteligentReference.href = UTIL.FormatString(CONSTANT_10.CONSTANT.URL_ANGULAR, element.FluentAggregation.Type);
                inteligentReference.classList.add('nav-link');
                inteligentReference.textContent = element.Name;
                inteligentReference.target = "_blank";
                render.InteligentReferenceList.appendChild(inteligentReference);
            };
            RenderClass.prototype.Dispose = function (render) {
                if (render != null) {
                    render.RootElement.remove();
                }
            };
            return RenderClass;
        }());
        exports.RenderClass = RenderClass;
    });
    
    'marker:resolver';

    function get_define(name) {
        if (defines[name]) {
            return defines[name];
        }
        else if (defines[name + '/index']) {
            return defines[name + '/index'];
        }
        else {
            var dependencies = ['exports'];
            var factory = function (exports) {
                try {
                    Object.defineProperty(exports, "__cjsModule", { value: true });
                    Object.defineProperty(exports, "default", { value: require(name) });
                }
                catch (_a) {
                    throw Error(['module "', name, '" not found.'].join(''));
                }
            };
            return { dependencies: dependencies, factory: factory };
        }
    }
    var instances = {};
    function resolve(name) {
        if (instances[name]) {
            return instances[name];
        }
        if (name === 'exports') {
            return {};
        }
        var define = get_define(name);
        instances[name] = {};
        var dependencies = define.dependencies.map(function (name) { return resolve(name); });
        define.factory.apply(define, dependencies);
        var exports = dependencies[define.dependencies.indexOf('exports')];
        instances[name] = (exports['__cjsModule']) ? exports["default"] : exports;
        return instances[name];
    }
    if (entry[0] !== null) {
        return resolve(entry[0]);
    }
})();