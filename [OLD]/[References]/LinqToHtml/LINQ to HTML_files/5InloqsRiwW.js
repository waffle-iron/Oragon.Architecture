/*1361377846,178142515*/

if (self.CavalryLogger) { CavalryLogger.start_js(["Ass+4"]); }

__d("OnloadHooks",["Arbiter","ErrorUtils","InitialJSLoader","OnloadEvent"],function(a,b,c,d,e,f){var g=b('Arbiter'),h=b('ErrorUtils'),i=b('InitialJSLoader'),j=b('OnloadEvent');function k(){var r=a.CavalryLogger;if(!window.loaded&&r)r.getInstance().setTimeStamp('t_prehooks');n('onloadhooks');if(!window.loaded&&r)r.getInstance().setTimeStamp('t_hooks');window.loaded=true;g.inform('uipage_onload',true,g.BEHAVIOR_STATE);}function l(){n('onafterloadhooks');window.afterloaded=true;}function m(r,s){return h.applyWithGuard(r,null,null,function(t){t.event_type=s;t.category='runhook';});}function n(r){var s=(r=='onbeforeleavehooks')||(r=='onbeforeunloadhooks');do{var t=window[r];if(!t)break;if(!s)window[r]=null;for(var u=0;u<t.length;u++){var v=m(t[u],r);if(s&&v)return v;}}while(!s&&window[r]);}function o(){if(!window.loaded){window.loaded=true;n('onloadhooks');}if(!window.afterloaded){window.afterloaded=true;n('onafterloadhooks');}}function p(){g.registerCallback(k,[j.ONLOAD_DOMCONTENT_CALLBACK,i.INITIAL_JS_READY]);g.registerCallback(l,[j.ONLOAD_DOMCONTENT_CALLBACK,j.ONLOAD_CALLBACK,i.INITIAL_JS_READY]);g.subscribe(j.ONBEFOREUNLOAD,function(r,s){s.warn=n('onbeforeleavehooks')||n('onbeforeunloadhooks');if(!s.warn){window.loaded=false;window.afterloaded=false;}},g.SUBSCRIBE_NEW);g.subscribe(j.ONUNLOAD,function(r,s){n('onunloadhooks');n('onafterunloadhooks');},g.SUBSCRIBE_NEW);}var q={_onloadHook:k,_onafterloadHook:l,runHook:m,runHooks:n,keepWindowSetAsLoaded:o};p();a.OnloadHooks=e.exports=q;});
__d("legacy:onload-action",["OnloadHooks"],function(a,b,c,d){var e=b('OnloadHooks');a._onloadHook=e._onloadHook;a._onafterloadHook=e._onafterloadHook;a.runHook=e.runHook;a.runHooks=e.runHooks;a.keep_window_set_as_loaded=e.keepWindowSetAsLoaded;},3);
__d("EagleEye",["Arbiter","Env","OnloadEvent","isInIframe"],function(a,b,c,d,e,f){var g=b('Arbiter'),h=b('Env'),i=b('OnloadEvent'),j=b('isInIframe'),k=h.eagleEyeConfig||{},l='_e_',m=(window.name||'').toString();if(m.length==7&&m.substr(0,3)==l){m=m.substr(3);}else{m=k.seed;if(!j())window.name=l+m;}var n=(window.location.protocol=='https:'&&document.cookie.match(/\bcsm=1/))?'; secure':'',o=l+m+'_',p=new Date(Date.now()+604800000).toGMTString(),q=window.location.hostname.replace(/^.*(facebook\..*)$/i,'$1'),r='; expires='+p+';path=/; domain='+q+n,s=0,t,u=k.sessionStorage&&a.sessionStorage,v=document.cookie.length,w=false,x=Date.now();function y(ca){return o+(s++)+'='+encodeURIComponent(ca)+r;}function z(){var ca=[],da=false,ea=0,fa=0;this.isEmpty=function(){return !ca.length;};this.enqueue=function(ga,ha){if(ha){ca.unshift(ga);}else ca.push(ga);};this.dequeue=function(){ca.shift();};this.peek=function(){return ca[0];};this.clear=function(ga){v=Math.min(v,document.cookie.length);if(!w&&(new Date()-x>60000))w=true;var ha=!ga&&(document.cookie.search(l)>=0),ia=!!h.cookie_header_limit,ja=h.cookie_count_limit||19,ka=h.cookie_header_limit||3950,la=ja-5,ma=ka-1000;while(!this.isEmpty()){var na=y(this.peek());if(ia&&(na.length>ka||(w&&na.length+v>ka))){this.dequeue();continue;}if((ha||ia)&&((document.cookie.length+na.length>ka)||(document.cookie.split(';').length>ja)))break;document.cookie=na;ha=true;this.dequeue();}var oa=Date.now();if(ga||!da&&ha&&((fa>0)&&(Math.min(10*Math.pow(2,fa-1),60000)+ea<oa))&&g.query(i.ONLOAD)&&(!this.isEmpty()||(document.cookie.length>ma)||(document.cookie.split(';').length>la))){var pa=new Image(),qa=this,ra=h.tracking_domain||'';da=true;pa.onload=function ua(){da=false;fa=0;qa.clear();};pa.onerror=pa.onabort=function ua(){da=false;ea=Date.now();fa++;};var sa=h.fb_isb?'&fb_isb='+h.fb_isb:'',ta='&__user='+h.user;pa.src=ra+'/ajax/nectar.php?asyncSignal='+(Math.floor(Math.random()*10000)+1)+sa+ta+'&'+(!ga?'':'s=')+oa;}};}t=new z();if(u){var aa=function(){var ca=0,da=ca;function ea(){var ha=sessionStorage.getItem('_e_ids');if(ha){var ia=(ha+'').split(';');if(ia.length==2){ca=parseInt(ia[0],10);da=parseInt(ia[1],10);}}}function fa(){var ha=ca+';'+da;sessionStorage.setItem('_e_ids',ha);}function ga(ha){return '_e_'+((ha!==undefined)?ha:ca++);}this.isEmpty=function(){return da===ca;};this.enqueue=function(ha,ia){var ja=ia?ga(--da):ga();sessionStorage.setItem(ja,ha);fa();};this.dequeue=function(){this.isEmpty();sessionStorage.removeItem(ga(da));da++;fa();};this.peek=function(){var ha=sessionStorage.getItem(ga(da));return ha?(ha+''):ha;};this.clear=t.clear;ea();};t=new aa();}var ba={log:function(ca,da,ea){if(h.no_cookies)return;var fa=[m,Date.now(),ca].concat(da);fa.push(fa.length);function ga(){var ha=JSON.stringify(fa);try{t.enqueue(ha,!!ea);t.clear(!!ea);}catch(ia){if(u&&(ia.code===1000)){t=new z();u=false;ga();}}}ga();},getSessionID:function(){return m;}};e.exports=ba;a.EagleEye=ba;},3);
__d("ClickRefUtils",[],function(a,b,c,d,e,f){var g={get_intern_ref:function(h){if(!!h){var i={profile_minifeed:1,gb_content_and_toolbar:1,gb_muffin_area:1,ego:1,bookmarks_menu:1,jewelBoxNotif:1,jewelNotif:1,BeeperBox:1,navSearch:1};for(var j=h;j&&j!=document.body;j=j.parentNode){if(!j.id||typeof j.id!=='string')continue;if(j.id.substr(0,8)=='pagelet_')return j.id.substr(8);if(j.id.substr(0,8)=='box_app_')return j.id;if(i[j.id])return j.id;}}return '-';},get_href:function(h){var i=(h.getAttribute&&(h.getAttribute('ajaxify')||h.getAttribute('data-endpoint'))||h.action||h.href||h.name);return typeof i==='string'?i:null;},should_report:function(h,i){if(i=='FORCE')return true;if(i=='INDIRECT')return false;return h&&(g.get_href(h)||(h.getAttribute&&h.getAttribute('data-ft')));}};e.exports=g;});
__d("getContextualParent",["ge"],function(a,b,c,d,e,f){var g=b('ge');function h(i,j){var k,l=false;do{if(i.getAttribute&&(k=i.getAttribute('data-ownerid'))){i=g(k);l=true;}else i=i.parentNode;}while(j&&i&&!l);return i;}e.exports=h;});
__d("collectDataAttributes",["getContextualParent"],function(a,b,c,d,e,f){var g=b('getContextualParent');function h(i,j){var k={},l={},m=j.length,n;for(n=0;n<m;++n){k[j[n]]={};l[j[n]]='data-'+j[n];}var o={tn:'',"tn-debug":','};while(i){if(i.getAttribute)for(n=0;n<m;++n){var p=i.getAttribute(l[j[n]]);if(p){var q=JSON.parse(p);for(var r in q)if(o[r]!==undefined){if(k[j[n]][r]===undefined)k[j[n]][r]=[];k[j[n]][r].push(q[r]);}else if(k[j[n]][r]===undefined)k[j[n]][r]=q[r];}}i=g(i);}for(var s in k)for(var t in o)if(k[s][t]!==undefined)k[s][t]=k[s][t].join(o[t]);return k;}e.exports=h;});
__d("setUECookie",["Env"],function(a,b,c,d,e,f){var g=b('Env');function h(i){if(!g.no_cookies){var j=0;if(a.afterloaded){j=2;}else if(a.loaded)j=1;document.cookie="act="+encodeURIComponent(i+":"+j)+"; path=/; domain="+window.location.hostname.replace(/^.*(\.facebook\..*)$/i,'$1');}}e.exports=h;});
__d("ClickRefLogger",["Arbiter","EagleEye","ClickRefUtils","collectDataAttributes","copyProperties","ge","setUECookie","$"],function(a,b,c,d,e,f){var g=b('Arbiter'),h=b('EagleEye'),i=b('ClickRefUtils'),j=b('collectDataAttributes'),k=b('copyProperties'),l=b('ge'),m=b('setUECookie'),n=b('$');function o(q){if(!l('content'))return [0,0,0,0];var r=n('content'),s=a.Vector2?a.Vector2.getEventPosition(q):{x:0,y:0};return [s.x,s.y,r.offsetLeft,r.clientWidth];}function p(q,r,event,s){var t=(!a.ArbiterMonitor)?'r':'a',u=[0,0,0,0],v,w,x;if(!!event){v=event.type;if(v=='click'&&l('content'))u=o(event);var y=0;event.ctrlKey&&(y+=1);event.shiftKey&&(y+=2);event.altKey&&(y+=4);event.metaKey&&(y+=8);if(y)v+=y;}if(!!r)w=i.get_href(r);var z=[];if(a.ArbiterMonitor){x=a.ArbiterMonitor.getInternRef(r);z=a.ArbiterMonitor.getActFields();}var aa=j(!!event?(event.target||event.srcElement):r,['ft','gt']);k(aa.ft,s.ft||{});k(aa.gt,s.gt||{});if(typeof(aa.ft.ei)==='string')delete aa.ft.ei;var ba=[q._ue_ts,q._ue_count,w||'-',q._context,v||'-',x||i.get_intern_ref(r),t,a.URI?a.URI.getRequestURI(true,true).getUnqualifiedURI().toString():location.pathname+location.search+location.hash,aa].concat(u).concat(z);return ba;}g.subscribe("ClickRefAction/new",function(q,r){if(i.should_report(r.node,r.mode)){var s=p(r.cfa,r.node,r.event,r.extra_data);m(r.cfa.ue);h.log('act',s);if(window.chromePerfExtension)window.postMessage({clickRef:JSON.stringify(s)},window.location.origin);}});});
__d("PostLoadJS",["Bootloader","Run","emptyFunction"],function(a,b,c,d,e,f){var g=b('Bootloader'),h=b('Run'),i=b('emptyFunction');function j(l,m){h.onAfterLoad(function(){g.loadModules.call(g,[l],m);});}var k={loadAndRequire:function(l){j(l,i);},loadAndCall:function(l,m,n){j(l,function(o){o[m].apply(o,n);});}};e.exports=k;});
__d("ScriptPathState",["Arbiter"],function(a,b,c,d,e,f){var g=b('Arbiter'),h,i,j,k,l=100,m={setIsUIPageletRequest:function(n){j=n;},setUserURISampleRate:function(n){k=n;},reset:function(){h=null;i=false;j=false;},_shouldUpdateScriptPath:function(){return (i&&!j);},_shouldSendURI:function(){return (Math.random()<k);},getParams:function(){var n={};if(m._shouldUpdateScriptPath()){if(m._shouldSendURI()&&h!==null)n.user_uri=h.substring(0,l);}else n.no_script_path=1;return n;}};g.subscribe("pre_page_transition",function(n,o){i=true;h=o.to.getUnqualifiedURI().toString();});e.exports=a.ScriptPathState=m;});
__d("throttle",[],function(a,b,c,d,e,f){function g(h,i,j,k){if(i==null)i=100;var l,m,n,o,p,q,r,s,t=0;function u(){r&&h.call(j,l,m,n,o,p);r=false;}return function v(w,x,y,z,aa){r=true;l=w;m=x;n=y;o=z;p=aa;q=Date.now();if(q-t>2*i){s=null;u();}if(!s){t=q;s=setTimeout(function(){s=null;u();},i,!k);}};}e.exports=g;});
__d("throttleAcrossTransitions",["throttle"],function(a,b,c,d,e,f){var g=b('throttle');function h(i,j,k){return g(i,j,k,true);}e.exports=h;});
__d("UserActionHistory",["Arbiter","ClickRefUtils","ScriptPath","throttleAcrossTransitions"],function(a,b,c,d,e,f){var g=b('Arbiter'),h=b('ClickRefUtils'),i=b('ScriptPath'),j=b('throttleAcrossTransitions'),k={click:1,submit:1},l=false,m={log:[],len:0},n=j(function(){try{l._ua_log=JSON.stringify(m);}catch(q){l=false;}},1000);function o(){try{if(a.sessionStorage){l=a.sessionStorage;l._ua_log&&(m=JSON.parse(l._ua_log));}}catch(q){l=false;}m.log[m.len%10]={ts:Date.now(),path:'-',index:m.len,type:'init',iref:'-'};m.len++;g.subscribe("UserAction/new",function(r,s){var t=s.ua,u=s.node,event=s.event;if(!event||!(event.type in k))return;var v={path:i.getScriptPath(),type:event.type,ts:t._ue_ts,iref:h.get_intern_ref(u)||'-',index:m.len};m.log[m.len++%10]=v;l&&n();});}function p(){return m.log.sort(function(q,r){return (r.ts!=q.ts)?(r.ts-q.ts):(r.index-q.index);});}o();e.exports={getHistory:p};});