:: netsh http delete urlacl url=http://+:8000/OragonEnterpriseLog
:: netsh http add urlacl url=http://+:8000/OragonEnterpriseLog user=\LogEngineSvcUsr
:: netsh http add urlacl url=http://+:8000/OragonEnterpriseLog user=imusica\luiz.faria


cd "C:\Projetos\Oragon\branches\iMusica2013\EnterpriseLog\[publish]\"
cd "LogEngineServer"
LogEngineServer.exe install
pause