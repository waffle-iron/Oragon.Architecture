/// <reference path="../linq.js" />
var Oragon = Oragon || {};
Oragon.Architecture = Oragon.Architecture || {};
Oragon.Architecture.Scripting = Oragon.Architecture.Scripting || {};

Oragon.Architecture.Scripting.TimerManager = {

	init: function () { 
		
	},

	run: function (timerConfig)
	{
		/* 
		timerConfig: {
			intervalInSeconds: 2,
			fn: function(timerConfig){ return true/false}
			autoReschedule: true/false
		}
		 */

		setTimeout(Oragon.Architecture.Scripting.TimerManager._runJob, timerConfig.intervalInSeconds * 1000, timerConfig);		
	},

	_runJob: function (innerTimerConfig) {
		var jobResult = innerTimerConfig.fn(innerTimerConfig);
		if (jobResult && innerTimerConfig.autoReschedule) {
			Oragon.Architecture.Scripting.TimerManager.run(innerTimerConfig)
		}
	}
	


};
Oragon.Architecture.Scripting.TimerManager.init();