/*! For license information please see LICENSES */
(window.webpackJsonp=window.webpackJsonp||[]).push([[23],{685:function(e,t,r){var n,o,h;o=[],n=function s(){"use strict";var e="undefined"!=typeof self?self:"undefined"!=typeof window?window:void 0!==e?e:{},t=!e.document&&!!e.postMessage,r=e.IS_PAPA_WORKER||!1,a={},u=0,b={parse:function(t,r){var n=(r=r||{}).dynamicTyping||!1;if(w(n)&&(r.dynamicTypingFunction=n,n={}),r.dynamicTyping=n,r.transform=!!w(r.transform)&&r.transform,r.worker&&b.WORKERS_SUPPORTED){var i=function(){if(!b.WORKERS_SUPPORTED)return!1;var t,i,r=(t=e.URL||e.webkitURL||null,i=s.toString(),b.BLOB_URL||(b.BLOB_URL=t.createObjectURL(new Blob(["var global = (function() { if (typeof self !== 'undefined') { return self; } if (typeof window !== 'undefined') { return window; } if (typeof global !== 'undefined') { return global; } return {}; })(); global.IS_PAPA_WORKER=true; ","(",i,")();"],{type:"text/javascript"})))),n=new e.Worker(r);return n.onmessage=m,n.id=u++,a[n.id]=n}();return i.userStep=r.step,i.userChunk=r.chunk,i.userComplete=r.complete,i.userError=r.error,r.step=w(r.step),r.chunk=w(r.chunk),r.complete=w(r.complete),r.error=w(r.error),delete r.worker,void i.postMessage({input:t,config:r,workerId:i.id})}var o=null;return b.NODE_STREAM_INPUT,"string"==typeof t?(t=function(e){return 65279===e.charCodeAt(0)?e.slice(1):e}(t),o=r.download?new h(r):new p(r)):!0===t.readable&&w(t.read)&&w(t.on)?o=new g(r):(e.File&&t instanceof File||t instanceof Object)&&(o=new l(r)),o.stream(t)},unparse:function(e,t){var r=!1,n=!0,o=",",h="\r\n",s='"',a=s+s,l=!1,i=null,d=!1;!function(){if("object"==typeof t){if("string"!=typeof t.delimiter||b.BAD_DELIMITERS.filter((function(e){return-1!==t.delimiter.indexOf(e)})).length||(o=t.delimiter),("boolean"==typeof t.quotes||"function"==typeof t.quotes||Array.isArray(t.quotes))&&(r=t.quotes),"boolean"!=typeof t.skipEmptyLines&&"string"!=typeof t.skipEmptyLines||(l=t.skipEmptyLines),"string"==typeof t.newline&&(h=t.newline),"string"==typeof t.quoteChar&&(s=t.quoteChar),"boolean"==typeof t.header&&(n=t.header),Array.isArray(t.columns)){if(0===t.columns.length)throw new Error("Option columns is empty");i=t.columns}void 0!==t.escapeChar&&(a=t.escapeChar+s),("boolean"==typeof t.escapeFormulae||t.escapeFormulae instanceof RegExp)&&(d=t.escapeFormulae instanceof RegExp?t.escapeFormulae:/^[=+\-@\t\r].*$/)}}();var u=new RegExp(c(s),"g");if("string"==typeof e&&(e=JSON.parse(e)),Array.isArray(e)){if(!e.length||Array.isArray(e[0]))return f(null,e,l);if("object"==typeof e[0])return f(i||Object.keys(e[0]),e,l)}else if("object"==typeof e)return"string"==typeof e.data&&(e.data=JSON.parse(e.data)),Array.isArray(e.data)&&(e.fields||(e.fields=e.meta&&e.meta.fields||i),e.fields||(e.fields=Array.isArray(e.data[0])?e.fields:"object"==typeof e.data[0]?Object.keys(e.data[0]):[]),Array.isArray(e.data[0])||"object"==typeof e.data[0]||(e.data=[e.data])),f(e.fields||[],e.data||[],l);throw new Error("Unable to serialize unrecognized input");function f(e,t,r){var i="";"string"==typeof e&&(e=JSON.parse(e)),"string"==typeof t&&(t=JSON.parse(t));var l=Array.isArray(e)&&0<e.length,s=!Array.isArray(t[0]);if(l&&n){for(var a=0;a<e.length;a++)0<a&&(i+=o),i+=m(e[a],a);0<t.length&&(i+=h)}for(var d=0;d<t.length;d++){var u=l?e.length:t[d].length,c=!1,f=l?0===Object.keys(t[d]).length:0===t[d].length;if(r&&!l&&(c="greedy"===r?""===t[d].join("").trim():1===t[d].length&&0===t[d][0].length),"greedy"===r&&l){for(var _=[],v=0;v<u;v++){var y=s?e[v]:v;_.push(t[d][y])}c=""===_.join("").trim()}if(!c){for(var p=0;p<u;p++){0<p&&!f&&(i+=o);var g=l&&s?e[p]:p;i+=m(t[d][g],p)}d<t.length-1&&(!r||0<u&&!f)&&(i+=h)}}return i}function m(e,t){if(null==e)return"";if(e.constructor===Date)return JSON.stringify(e).slice(1,25);var n=!1;d&&"string"==typeof e&&d.test(e)&&(e="'"+e,n=!0);var i=e.toString().replace(u,a);return(n=n||!0===r||"function"==typeof r&&r(e,t)||Array.isArray(r)&&r[t]||function(e,t){for(var r=0;r<t.length;r++)if(-1<e.indexOf(t[r]))return!0;return!1}(i,b.BAD_DELIMITERS)||-1<i.indexOf(o)||" "===i.charAt(0)||" "===i.charAt(i.length-1))?s+i+s:i}}};if(b.RECORD_SEP=String.fromCharCode(30),b.UNIT_SEP=String.fromCharCode(31),b.BYTE_ORDER_MARK="\ufeff",b.BAD_DELIMITERS=["\r","\n",'"',b.BYTE_ORDER_MARK],b.WORKERS_SUPPORTED=!t&&!!e.Worker,b.NODE_STREAM_INPUT=1,b.LocalChunkSize=10485760,b.RemoteChunkSize=5242880,b.DefaultDelimiter=",",b.Parser=f,b.ParserHandle=d,b.NetworkStreamer=h,b.FileStreamer=l,b.StringStreamer=p,b.ReadableStreamStreamer=g,e.jQuery){var n=e.jQuery;n.fn.parse=function(t){var r=t.config||{},u=[];return this.each((function(t){if("INPUT"!==n(this).prop("tagName").toUpperCase()||"file"!==n(this).attr("type").toLowerCase()||!e.FileReader||!this.files||0===this.files.length)return!0;for(var o=0;o<this.files.length;o++)u.push({file:this.files[o],inputElem:this,instanceConfig:n.extend({},r)})})),o(),this;function o(){if(0!==u.length){var e,r,o,i,l=u[0];if(w(t.before)){var s=t.before(l.file,l.inputElem);if("object"==typeof s){if("abort"===s.action)return e="AbortError",r=l.file,o=l.inputElem,i=s.reason,void(w(t.error)&&t.error({name:e},r,o,i));if("skip"===s.action)return void h();"object"==typeof s.config&&(l.instanceConfig=n.extend(l.instanceConfig,s.config))}else if("skip"===s)return void h()}var a=l.instanceConfig.complete;l.instanceConfig.complete=function(e){w(a)&&a(e,l.file,l.inputElem),h()},b.parse(l.file,l.instanceConfig)}else w(t.complete)&&t.complete()}function h(){u.splice(0,1),o()}}}function o(t){this._handle=null,this._finished=!1,this._completed=!1,this._halted=!1,this._input=null,this._baseIndex=0,this._partialLine="",this._rowCount=0,this._start=0,this._nextChunk=null,this.isFirstChunk=!0,this._completeResults={data:[],errors:[],meta:{}},function(e){var t=y(e);t.chunkSize=parseInt(t.chunkSize),e.step||e.chunk||(t.chunkSize=null),this._handle=new d(t),(this._handle.streamer=this)._config=t}.call(this,t),this.parseChunk=function(t,n){if(this.isFirstChunk&&w(this._config.beforeFirstChunk)){var o=this._config.beforeFirstChunk(t);void 0!==o&&(t=o)}this.isFirstChunk=!1,this._halted=!1;var i=this._partialLine+t;this._partialLine="";var h=this._handle.parse(i,this._baseIndex,!this._finished);if(!this._handle.paused()&&!this._handle.aborted()){var s=h.meta.cursor;this._finished||(this._partialLine=i.substring(s-this._baseIndex),this._baseIndex=s),h&&h.data&&(this._rowCount+=h.data.length);var a=this._finished||this._config.preview&&this._rowCount>=this._config.preview;if(r)e.postMessage({results:h,workerId:b.WORKER_ID,finished:a});else if(w(this._config.chunk)&&!n){if(this._config.chunk(h,this._handle),this._handle.paused()||this._handle.aborted())return void(this._halted=!0);h=void 0,this._completeResults=void 0}return this._config.step||this._config.chunk||(this._completeResults.data=this._completeResults.data.concat(h.data),this._completeResults.errors=this._completeResults.errors.concat(h.errors),this._completeResults.meta=h.meta),this._completed||!a||!w(this._config.complete)||h&&h.meta.aborted||(this._config.complete(this._completeResults,this._input),this._completed=!0),a||h&&h.meta.paused||this._nextChunk(),h}this._halted=!0},this._sendError=function(t){w(this._config.error)?this._config.error(t):r&&this._config.error&&e.postMessage({workerId:b.WORKER_ID,error:t,finished:!1})}}function h(e){var i;(e=e||{}).chunkSize||(e.chunkSize=b.RemoteChunkSize),o.call(this,e),this._nextChunk=t?function(){this._readChunk(),this._chunkLoaded()}:function(){this._readChunk()},this.stream=function(e){this._input=e,this._nextChunk()},this._readChunk=function(){if(this._finished)this._chunkLoaded();else{if(i=new XMLHttpRequest,this._config.withCredentials&&(i.withCredentials=this._config.withCredentials),t||(i.onload=k(this._chunkLoaded,this),i.onerror=k(this._chunkError,this)),i.open(this._config.downloadRequestBody?"POST":"GET",this._input,!t),this._config.downloadRequestHeaders){var e=this._config.downloadRequestHeaders;for(var r in e)i.setRequestHeader(r,e[r])}if(this._config.chunkSize){var n=this._start+this._config.chunkSize-1;i.setRequestHeader("Range","bytes="+this._start+"-"+n)}try{i.send(this._config.downloadRequestBody)}catch(e){this._chunkError(e.message)}t&&0===i.status&&this._chunkError()}},this._chunkLoaded=function(){4===i.readyState&&(i.status<200||400<=i.status?this._chunkError():(this._start+=this._config.chunkSize?this._config.chunkSize:i.responseText.length,this._finished=!this._config.chunkSize||this._start>=function(e){var t=e.getResponseHeader("Content-Range");return null===t?-1:parseInt(t.substring(t.lastIndexOf("/")+1))}(i),this.parseChunk(i.responseText)))},this._chunkError=function(e){var t=i.statusText||e;this._sendError(new Error(t))}}function l(e){var i,t;(e=e||{}).chunkSize||(e.chunkSize=b.LocalChunkSize),o.call(this,e);var s="undefined"!=typeof FileReader;this.stream=function(e){this._input=e,t=e.slice||e.webkitSlice||e.mozSlice,s?((i=new FileReader).onload=k(this._chunkLoaded,this),i.onerror=k(this._chunkError,this)):i=new FileReaderSync,this._nextChunk()},this._nextChunk=function(){this._finished||this._config.preview&&!(this._rowCount<this._config.preview)||this._readChunk()},this._readChunk=function(){var e=this._input;if(this._config.chunkSize){var r=Math.min(this._start+this._config.chunkSize,this._input.size);e=t.call(e,this._start,r)}var n=i.readAsText(e,this._config.encoding);s||this._chunkLoaded({target:{result:n}})},this._chunkLoaded=function(e){this._start+=this._config.chunkSize,this._finished=!this._config.chunkSize||this._start>=this._input.size,this.parseChunk(e.target.result)},this._chunkError=function(){this._sendError(i.error)}}function p(e){var t;o.call(this,e=e||{}),this.stream=function(e){return t=e,this._nextChunk()},this._nextChunk=function(){if(!this._finished){var e,r=this._config.chunkSize;return r?(e=t.substring(0,r),t=t.substring(r)):(e=t,t=""),this._finished=!t,this.parseChunk(e)}}}function g(e){o.call(this,e=e||{});var t=[],r=!0,i=!1;this.pause=function(){o.prototype.pause.apply(this,arguments),this._input.pause()},this.resume=function(){o.prototype.resume.apply(this,arguments),this._input.resume()},this.stream=function(e){this._input=e,this._input.on("data",this._streamData),this._input.on("end",this._streamEnd),this._input.on("error",this._streamError)},this._checkIsFinished=function(){i&&1===t.length&&(this._finished=!0)},this._nextChunk=function(){this._checkIsFinished(),t.length?this.parseChunk(t.shift()):r=!0},this._streamData=k((function(e){try{t.push("string"==typeof e?e:e.toString(this._config.encoding)),r&&(r=!1,this._checkIsFinished(),this.parseChunk(t.shift()))}catch(e){this._streamError(e)}}),this),this._streamError=k((function(e){this._streamCleanUp(),this._sendError(e)}),this),this._streamEnd=k((function(){this._streamCleanUp(),i=!0,this._streamData("")}),this),this._streamCleanUp=k((function(){this._input.removeListener("data",this._streamData),this._input.removeListener("end",this._streamEnd),this._input.removeListener("error",this._streamError)}),this)}function d(e){var a,t,u,i=Math.pow(2,53),r=-i,s=/^\s*-?(\d+\.?|\.\d+|\d+\.\d+)([eE][-+]?\d+)?\s*$/,n=/^((\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z)))$/,o=this,h=0,l=0,d=!1,m=!1,_=[],v={data:[],errors:[],meta:{}};if(w(e.step)){var p=e.step;e.step=function(r){if(v=r,E())g();else{if(g(),0===v.data.length)return;h+=r.data.length,e.preview&&h>e.preview?t.abort():(v.data=v.data[0],p(v,o))}}}function k(t){return"greedy"===e.skipEmptyLines?""===t.join("").trim():1===t.length&&0===t[0].length}function g(){return v&&u&&(R("Delimiter","UndetectableDelimiter","Unable to auto-detect delimiting character; defaulted to '"+b.DefaultDelimiter+"'"),u=!1),e.skipEmptyLines&&(v.data=v.data.filter((function(e){return!k(e)}))),E()&&function(){if(v)if(Array.isArray(v.data[0])){for(var t=0;E()&&t<v.data.length;t++)v.data[t].forEach(r);v.data.splice(0,1)}else v.data.forEach(r);function r(t,r){w(e.transformHeader)&&(t=e.transformHeader(t,r)),_.push(t)}}(),function(){if(!v||!e.header&&!e.dynamicTyping&&!e.transform)return v;function t(t,r){var n,i=e.header?{}:[];for(n=0;n<t.length;n++){var o=n,s=t[n];e.header&&(o=n>=_.length?"__parsed_extra":_[n]),e.transform&&(s=e.transform(s,o)),s=C(o,s),"__parsed_extra"===o?(i[o]=i[o]||[],i[o].push(s)):i[o]=s}return e.header&&(n>_.length?R("FieldMismatch","TooManyFields","Too many fields: expected "+_.length+" fields but parsed "+n,l+r):n<_.length&&R("FieldMismatch","TooFewFields","Too few fields: expected "+_.length+" fields but parsed "+n,l+r)),i}var r=1;return!v.data.length||Array.isArray(v.data[0])?(v.data=v.data.map(t),r=v.data.length):v.data=t(v.data,0),e.header&&v.meta&&(v.meta.fields=_),l+=r,v}()}function E(){return e.header&&0===_.length}function C(t,o){return h=t,e.dynamicTypingFunction&&void 0===e.dynamicTyping[h]&&(e.dynamicTyping[h]=e.dynamicTypingFunction(h)),!0===(e.dynamicTyping[h]||e.dynamicTyping)?"true"===o||"TRUE"===o||"false"!==o&&"FALSE"!==o&&(function(e){if(s.test(e)){var t=parseFloat(e);if(r<t&&t<i)return!0}return!1}(o)?parseFloat(o):n.test(o)?new Date(o):""===o?null:o):o;var h}function R(e,t,r,i){var n={type:e,code:t,message:r};void 0!==i&&(n.row=i),v.errors.push(n)}this.parse=function(r,n,o){var i=e.quoteChar||'"';if(e.newline||(e.newline=function(e,t){e=e.substring(0,1048576);var r=new RegExp(c(t)+"([^]*?)"+c(t),"gm"),i=(e=e.replace(r,"")).split("\r"),n=e.split("\n"),s=1<n.length&&n[0].length<i[0].length;if(1===i.length||s)return"\n";for(var a=0,o=0;o<i.length;o++)"\n"===i[o][0]&&a++;return a>=i.length/2?"\r\n":"\r"}(r,i)),u=!1,e.delimiter)w(e.delimiter)&&(e.delimiter=e.delimiter(r),v.meta.delimiter=e.delimiter);else{var h=function(t,r,n,i,o){var s,a,h,u;o=o||[",","\t","|",";",b.RECORD_SEP,b.UNIT_SEP];for(var l=0;l<o.length;l++){var d=o[l],c=0,m=0,_=0;h=void 0;for(var p=new f({comments:i,delimiter:d,newline:r,preview:10}).parse(t),g=0;g<p.data.length;g++)if(n&&k(p.data[g]))_++;else{var v=p.data[g].length;m+=v,void 0!==h?0<v&&(c+=Math.abs(v-h),h=v):h=v}0<p.data.length&&(m/=p.data.length-_),(void 0===a||c<=a)&&(void 0===u||u<m)&&1.99<m&&(a=c,s=d,u=m)}return{successful:!!(e.delimiter=s),bestDelimiter:s}}(r,e.newline,e.skipEmptyLines,e.comments,e.delimitersToGuess);h.successful?e.delimiter=h.bestDelimiter:(u=!0,e.delimiter=b.DefaultDelimiter),v.meta.delimiter=e.delimiter}var s=y(e);return e.preview&&e.header&&s.preview++,a=r,t=new f(s),v=t.parse(a,n,o),g(),d?{meta:{paused:!0}}:v||{meta:{paused:!1}}},this.paused=function(){return d},this.pause=function(){d=!0,t.abort(),a=w(e.chunk)?"":a.substring(t.getCharIndex())},this.resume=function(){o.streamer._halted?(d=!1,o.streamer.parseChunk(a,!0)):setTimeout(o.resume,3)},this.aborted=function(){return m},this.abort=function(){m=!0,t.abort(),v.meta.aborted=!0,w(e.complete)&&e.complete(v),a=""}}function c(e){return e.replace(/[.*+?^${}()|[\]\\]/g,"\\$&")}function f(e){var t,r=(e=e||{}).delimiter,n=e.newline,o=e.comments,q=e.step,h=e.preview,l=e.fastMode,d=t=void 0===e.quoteChar||null===e.quoteChar?'"':e.quoteChar;if(void 0!==e.escapeChar&&(d=e.escapeChar),("string"!=typeof r||-1<b.BAD_DELIMITERS.indexOf(r))&&(r=","),o===r)throw new Error("Comment character same as delimiter");!0===o?o="#":("string"!=typeof o||-1<b.BAD_DELIMITERS.indexOf(o))&&(o=!1),"\n"!==n&&"\r"!==n&&"\r\n"!==n&&(n="\n");var f=0,m=!1;this.parse=function(i,_,v){if("string"!=typeof i)throw new Error("Input must be a string");var y=i.length,k=r.length,s=n.length,a=o.length,E=w(q),u=[],C=[],R=[],S=f=0;if(!i)return J();if(e.header&&!_){var O=i.split(n)[0].split(r),x=[],p={},g=!1;for(var F in O){var A=O[F];w(e.transformHeader)&&(A=e.transformHeader(A,F));var D=A,I=p[A]||0;for(0<I&&(g=!0,D=A+"_"+I),p[A]=I+1;x.includes(D);)D=D+"_"+I;x.push(D)}if(g){var L=i.split(n);L[0]=x.join(r),i=L.join(n)}}if(l||!1!==l&&-1===i.indexOf(t)){for(var b=i.split(n),T=0;T<b.length;T++){if(R=b[T],f+=R.length,T!==b.length-1)f+=n.length;else if(v)return J();if(!o||R.substring(0,a)!==o){if(E){if(u=[],H(R.split(r)),Q(),m)return J()}else H(R.split(r));if(h&&h<=T)return u=u.slice(0,h),J(!0)}}return J()}for(var j=i.indexOf(r,f),M=i.indexOf(n,f),P=new RegExp(c(d)+c(t),"g"),z=i.indexOf(t,f);;)if(i[f]!==t)if(o&&0===R.length&&i.substring(f,f+a)===o){if(-1===M)return J();f=M+s,M=i.indexOf(n,f),j=i.indexOf(r,f)}else if(-1!==j&&(j<M||-1===M))R.push(i.substring(f,j)),f=j+k,j=i.indexOf(r,f);else{if(-1===M)break;if(R.push(i.substring(f,M)),W(M+s),E&&(Q(),m))return J();if(h&&u.length>=h)return J(!0)}else for(z=f,f++;;){if(-1===(z=i.indexOf(t,z+1)))return v||C.push({type:"Quotes",code:"MissingQuotes",message:"Quoted field unterminated",row:u.length,index:f}),K();if(z===y-1)return K(i.substring(f,z).replace(P,t));if(t!==d||i[z+1]!==d){if(t===d||0===z||i[z-1]!==d){-1!==j&&j<z+1&&(j=i.indexOf(r,z+1)),-1!==M&&M<z+1&&(M=i.indexOf(n,z+1));var U=B(-1===M?j:Math.min(j,M));if(i.substr(z+1+U,k)===r){R.push(i.substring(f,z).replace(P,t)),i[f=z+1+U+k]!==t&&(z=i.indexOf(t,f)),j=i.indexOf(r,f),M=i.indexOf(n,f);break}var N=B(M);if(i.substring(z+1+N,z+1+N+s)===n){if(R.push(i.substring(f,z).replace(P,t)),W(z+1+N+s),j=i.indexOf(r,f),z=i.indexOf(t,f),E&&(Q(),m))return J();if(h&&u.length>=h)return J(!0);break}C.push({type:"Quotes",code:"InvalidQuotes",message:"Trailing quote on quoted field is malformed",row:u.length,index:f}),z++}}else z++}return K();function H(e){u.push(e),S=f}function B(e){var t=0;if(-1!==e){var r=i.substring(z+1,e);r&&""===r.trim()&&(t=r.length)}return t}function K(e){return v||(void 0===e&&(e=i.substring(f)),R.push(e),f=y,H(R),E&&Q()),J()}function W(e){f=e,H(R),R=[],M=i.indexOf(n,f)}function J(e){return{data:u,errors:C,meta:{delimiter:r,linebreak:n,aborted:m,truncated:!!e,cursor:S+(_||0)}}}function Q(){q(J()),u=[],C=[]}},this.abort=function(){m=!0},this.getCharIndex=function(){return f}}function m(e){var t=e.data,r=a[t.workerId],i=!1;if(t.error)r.userError(t.error,t.file);else if(t.results&&t.results.data){var n={abort:function(){i=!0,_(t.workerId,{data:[],errors:[],meta:{aborted:!0}})},pause:v,resume:v};if(w(r.userStep)){for(var s=0;s<t.results.data.length&&(r.userStep({data:t.results.data[s],errors:t.results.errors,meta:t.results.meta},n),!i);s++);delete t.results}else w(r.userChunk)&&(r.userChunk(t.results,n,t.file),delete t.results)}t.finished&&!i&&_(t.workerId,t.results)}function _(e,t){var r=a[e];w(r.userComplete)&&r.userComplete(t),r.terminate(),delete a[e]}function v(){throw new Error("Not implemented.")}function y(e){if("object"!=typeof e||null===e)return e;var t=Array.isArray(e)?[]:{};for(var r in e)t[r]=y(e[r]);return t}function k(e,t){return function(){e.apply(t,arguments)}}function w(e){return"function"==typeof e}return r&&(e.onmessage=function(t){var r=t.data;if(void 0===b.WORKER_ID&&r&&(b.WORKER_ID=r.workerId),"string"==typeof r.input)e.postMessage({workerId:b.WORKER_ID,results:b.parse(r.input,r.config),finished:!0});else if(e.File&&r.input instanceof File||r.input instanceof Object){var n=b.parse(r.input,r.config);n&&e.postMessage({workerId:b.WORKER_ID,results:n,finished:!0})}}),(h.prototype=Object.create(o.prototype)).constructor=h,(l.prototype=Object.create(o.prototype)).constructor=l,(p.prototype=Object.create(p.prototype)).constructor=p,(g.prototype=Object.create(o.prototype)).constructor=g,b},void 0===(h="function"==typeof n?n.apply(t,o):n)||(e.exports=h)},917:function(e,t,r){"use strict";r.r(t);var n=r(30),o=(r(2),r(12),r(55),r(75),r(685)),h=r.n(o),l={data:function(){return{csvData:[],csvHeaders:[],isLoading:!1,uploadProgress:0,selectedFile:null}},methods:{handleFileSelect:function(e){this.selectedFile=e.target.files[0]},handleFileUpload2:function(e){var t=this;return Object(n.a)(regeneratorRuntime.mark((function r(){var o,l;return regeneratorRuntime.wrap((function(r){for(;;)switch(r.prev=r.next){case 0:return t.isLoading=!0,t.uploadProgress=0,o=e.target.files[0],r.next=5,t.getCSVRowCount(o);case 5:l=r.sent,t.selectedFile=e.target.files[0],h.a.parse(o,{header:!0,skipEmptyLines:!0,complete:function(e){t.csvData=e.data,t.csvHeaders=e.meta.fields,console.log("All rows processed."),t.isLoading=!1},step:function(){var e=Object(n.a)(regeneratorRuntime.mark((function e(r,n){var o;return regeneratorRuntime.wrap((function(e){for(;;)switch(e.prev=e.next){case 0:try{console.log(r.data.Code),o={code:r.data.Code,name:r.data.Name,january:parseFloat(r.data.January),february:parseFloat(r.data.February),march:parseFloat(r.data.March),april:parseFloat(r.data.April),may:parseFloat(r.data.May),june:parseFloat(r.data.June),july:parseFloat(r.data.July),august:parseFloat(r.data.August),september:parseFloat(r.data.September),october:parseFloat(r.data.October),november:parseFloat(r.data.November),december:parseFloat(r.data.December)},console.log("Data sent successfully:",o),"Accountant/AccountingImport",t.$axios.post("Accountant/AccountingImport",o),t.uploadProgress=Math.round(n.meta.lines/l*100),console.log("Data sent successfully:",o)}catch(e){console.error("Error sending data:",e.response?e.response.data:e.message),n.abort()}case 1:case"end":return e.stop()}}),e)})));return function(t,r){return e.apply(this,arguments)}}(),error:function(e){console.error("Error parsing CSV:",e),t.isLoading=!1}});case 8:case"end":return r.stop()}}),r)})))()},getCSVRowCount:function(e){return Object(n.a)(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return t.abrupt("return",new Promise((function(t,r){var n=new FileReader;n.onload=function(e){var r=e.target.result.split(/\r\n|\n|\r/).length;t(r)},n.onerror=function(){r(new Error("Error al leer el archivo CSV"))},n.readAsText(e)})));case 1:case"end":return t.stop()}}),t)})))()},handleFileUpload:function(e){var t=this,r=e.target.files[0];h.a.parse(r,{header:!0,skipEmptyLines:!0,complete:function(e){t.csvData=e.data,t.csvHeaders=e.meta.fields},error:function(e){console.error("Error parsing CSV:",e)}})}}},d=l,c=r(18),component=Object(c.a)(d,(function(){var e=this,t=e._self._c;return t("div",[t("input",{attrs:{type:"file"},on:{change:e.handleFileUpload2}}),e._v(" "),e.csvData.length?t("div",[t("table",[t("thead",[t("tr",e._l(e.csvHeaders,(function(header){return t("th",[e._v(e._s(header))])})),0)]),e._v(" "),t("tbody",e._l(e.csvData,(function(r){return t("tr",e._l(r,(function(r){return t("td",[e._v(e._s(r))])})),0)})),0)]),e._v(" "),t("button",{attrs:{disabled:!e.selectedFile||e.isLoading},on:{click:e.handleUpload}},[e._v("\n      "+e._s(e.isLoading?"Cargando...":"Cargar CSV")+"\n    ")]),e._v(" "),e.isLoading?t("div",[t("progress",{attrs:{max:"100"},domProps:{value:e.uploadProgress}}),e._v(" "),t("p",[e._v(e._s(e.uploadProgress)+"%")])]):e._e()]):e._e()])}),[],!1,null,null,null);t.default=component.exports}}]);