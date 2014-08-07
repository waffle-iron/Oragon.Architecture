var Oragon = Oragon || {};
Oragon.Architecture = Oragon.Architecture || {};
Oragon.Architecture.Scripting = Oragon.Architecture.Scripting || {};

Oragon.Architecture.Scripting.ErrorManager = {
	init: function () {
		radio('AjaxManager:EndRequest').subscribe(function (resultInfo) {
			var configurationFactory = Oragon.Architecture.Scripting.ErrorManager.getConfigurationFactory(resultInfo);
			if(Ext.isEmpty(configurationFactory)) return;
			var configuration = configurationFactory(resultInfo);
			if(Ext.isEmpty(configuration)) return;
			Oragon.Architecture.Scripting.ErrorManager.handle(configuration, resultInfo);
		});
	},
	handle: function(configuration, resultInfo)
	{
		var detailField = null;
		if (configuration.viewHtml == false) {
			detailField = {
				xtype: 'textarea',
				fieldLabel: 'Detalhes Técnicos',
				value: configuration.messageDetails,
				readOnly: true,
				style: {
					width: '100%',
					height: '100%'
				}
			};
		} else {
			detailField = {
				xtype: 'htmleditor',
				value: configuration.messageDetails,
				readOnly: true,
				style: {
					width: '100%',
					height: '100%'
				}
			};
		}

		var win = Ext.create('widget.window', {
			title: "Ocorreu um erro no processamento",
			closable: true,
			//maximizable: true,
			modal: true,
			width: 600,
			height: 280,
			layout: 'fit',
			bodyStyle: 'padding: 5px;',
			items: [
				Ext.create('Ext.tab.Panel', {
					tabPosition: 'bottom',
					items: [{
						title: 'Informações',
						bodyStyle: 'padding: 5px;',
						layout: 'fit',
						items: [
							{
								xtype: 'displayfield',
								value: configuration.messageText + "<br><br><br><br><br><br><br><br><br><br><br><span style='font-size:9px'>Agora você também tem a opção de informar este problema automaticamente. Clique em 'Detalhes técnicos', em seguinda preencha o formulário e nos informe sobre esta instabilidade.</span>"
							}
						]
					}, {
						title: 'Detalhes técnicos',
						tabConfig: {
							title: 'Detalhes técnicos',
							tooltip: 'A button tooltip'
						},
						bodyStyle: 'padding: 5px;',
						layout: 'vbox',
						items: [
							{
								xtype: 'textfield',
								fieldLabel: 'Descrição',
								id: 'txtDescricaoErro',
								width: 564
							},
							detailField
						],
						buttons: [
							{
								text: 'Enviar informações sobre o Erro',
								width: 200,
								handler: function () {
									/*
									Oragon.Architecture.Scripting.AjaxManager.sendAndReceiveUsingJson({
										waitingMessage: "Enviando Bug Reporte...",
										urlToSend: UrlManager.getUrl('Controls', 'BugReport', 'EnviarReport'),
										params: {
											bugReport: {
												EnderecoUrl: serverObj.url,
												Parametros: serverObj.params,
												Navegador: Ext.userAgent,
												Home: Ext.encode(mainRoute),
												RotaAtual: Ext.encode(currentRoute),
												MensagemDeErro: serverObj.ExceptionDetails,
												MensagemDoUsuario: Ext.getCmp('txtDescricaoErro').getValue()
											}
										},
										success: function (data) {
											win.close();
										},
										showMessageOnSuccess: true
									});
									*/
								}
							}
						]
					}]
				})
			]
		});
		win.show();

		/*if(Ext.isEmpty(configuration.messageDetails))
		{
			Ext.MessageBox.show({
				title: configuration.messageTitle,
				buttons: Ext.MessageBox.OK,
				closable: false,
				msg: configuration.messageText,
				icon: configuration.messageIcon
			});
		}*/
	},

	getConfigurationFactory: function (resultInfo) {
		var currentHandler = null;
		if(resultInfo.httpStatusCode == 0)
		{
			currentHandler = function(resultInfo)
			{
				return {
					status : 1,
					messageTitle : 'Server Unavailable or Address Error',
					messageIcon: Ext.MessageBox.ERROR,
					messageText : Ext.String.format('The server did not respond to the address "{0}".', resultInfo.configObject.urlToSend),
					messageDetails : null,
					viewHtml: false
				};
			};
		}
		return currentHandler;
	}

	/*getMesageBoxConfigFromStatus: function (status) {
		var returnObject = {
			messageTitle: null,
			messageIcon: null
		};
		switch (status) {
			case 0: //success
				returnObject.messageTitle = "successo";
				returnObject.messageIcon = Ext.MessageBox.INFO;
				break;
			case 1: //Error
				returnObject.messageTitle = "Erro";
				returnObject.messageIcon = Ext.MessageBox.ERROR;
				break;
			case 2: //Warning
				returnObject.messageTitle = "Aviso";
				returnObject.messageIcon = Ext.MessageBox.WARNING;
				break;
			case 3: //Information
				returnObject.messageTitle = "Informação";
				returnObject.messageIcon = Ext.MessageBox.INFO;
				break;
			case 4: //Question
				returnObject.messageTitle = "Questionamento";
				returnObject.messageIcon = Ext.MessageBox.QUESTION;
				break;
		}
		return returnObject;
	},
	showErrorWindow: function (serverObj, viewHtml) {
		var detailField = null;
		if (!viewHtml) {
			detailField = {
				xtype: 'textarea',
				fieldLabel: 'Detalhes Técnicos',
				value: serverObj.ExceptionDetails,
				readOnly: true
			};
		} else {
			detailField = {
				xtype: 'htmleditor',
				value: serverObj.ExceptionDetails,
				readOnly: true
			};
		}
		var win = Ext.create('widget.window', {
			title: "Ocorreu um erro no processamento",
			closable: true,
			//maximizable: true,
			modal: true,
			width: 600,
			height: 280,
			layout: 'fit',
			bodyStyle: 'padding: 5px;',
			items: [
				Ext.create('Ext.tab.Panel', {
					tabPosition: 'bottom',
					items: [{
						title: 'Informações',
						bodyStyle: 'padding: 5px;',
						layout: 'fit',
						items: [
							{
								xtype: 'displayfield',
								value: serverObj.MessageText + "<br><br><br><br><br><br><br><br><br><br><br><span style='font-size:9px'>Agora você também tem a opção de informar este problema automaticamente. Clique em 'Detalhes técnicos', em seguinda preencha o formulário e nos informe sobre esta instabilidade.</span>"
							}
						]
					}, {
						title: 'Detalhes técnicos',
						tabConfig: {
							title: 'Detalhes técnicos',
							tooltip: 'A button tooltip'
						},
						bodyStyle: 'padding: 5px;',
						layout: 'vbox',
						items: [
							{
								xtype: 'textfield',
								fieldLabel: 'Descrição',
								id: 'txtDescricaoErro',
								width: 564
							},
							detailField
						],
						buttons: [
							{
								text: 'Enviar informações sobre o Erro',
								width: 200,
								handler: function () {
									var mainRoute = window.Route;
									var currentRoute = window.Route;
									var currentWindow = window;
									while (currentWindow != currentWindow.parent) {
										if (currentWindow.parent == currentWindow)
											mainRoute = currentWindow.Route;
										currentWindow = currentWindow.parent;
									}

									Oragon.Architecture.Scripting.AjaxManager.sendAndReceiveUsingJson({
										waitingMessage: "Enviando Bug Reporte...",
										urlToSend: UrlManager.getUrl('Controls', 'BugReport', 'EnviarReport'),
										params: {
											bugReport: {
												EnderecoUrl: serverObj.url,
												Parametros: serverObj.params,
												Navegador: Ext.userAgent,
												Home: Ext.encode(mainRoute),
												RotaAtual: Ext.encode(currentRoute),
												MensagemDeErro: serverObj.ExceptionDetails,
												MensagemDoUsuario: Ext.getCmp('txtDescricaoErro').getValue()
											}
										},
										success: function (data) {
											win.close();
										},
										showMessageOnSuccess: true
									});
								}
							}
						]
					}]
				})
			]
		});
		win.show();
	},
	defaultErrorHandle: function (serverObj) {
		var messageConfig = Oragon.Architecture.Scripting.ErrorManager.getMesageBoxConfigFromStatus(serverObj.Status);
		Ext.MessageBox.show({
			title: messageConfig.messageTitle,
			buttons: Ext.MessageBox.OK,
			closable: false,
			msg: serverObj.messageText,
			icon: messageConfig.messageIcon
		});
	},

	*/
};
Oragon.Architecture.Scripting.ErrorManager.init();