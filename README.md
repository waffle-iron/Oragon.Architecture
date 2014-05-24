<p style="text-align: justify;"><img class="size-full wp-image-50 aligncenter" src="http://luizcarlosfaria.net/wp-content/uploads/2014/03/OragonArchitecture.export.png" alt="Oragon Architecture" width="613" height="127" /></p>

<h3 style="text-align: justify;">O que é?</h3>
<p style="text-align: justify;">Oragon Architecture, é um Application Framework, Open Source, de autoria de minha autoria. Comecei o projeto com código fechado, em 2007, após anos usando uma base consistente de frameworks e padrões para desenvolver projetos .Net. A intenção inicial era ter uma base de código pronta, para reduzir o tempo de codificação da infraestrutura de mecanismos das aplicações que eu construía. Com o passar dos meses, o projeto foi crescendo até poder ser chamado de Application Framework, organizando e orquestrando uma série de padrões e mecanismos diversos. Desde o início, a proposta do projeto é entregar robustez e absorver a complexidade na hora de integrar frameworks e padrões. A proposta sempre foi deixar o código das aplicações o mais simples possível, deixando a cargo do Oragon Architecture, toda a responsabilidade por gerir a infraestrutura rotineira no desenvolvimento de aplicações de negócio.</p>
<div id="attachment_88" style="width: 382px;" ><img class="size-full wp-image-88" src="http://luizcarlosfaria.net/wp-content/uploads/2014/03/Oragon-Architecture-Penknife.export.png" alt="Oragon Architecture" width="372" height="371"><p style="float: right !important;display:block !important;">Oragon Architecture</p></div>
<h3 style="text-align: justify;"></h3>
<h3 style="text-align: justify;">A evolução do projeto</h3>
<h4 style="text-align: justify;">A adoção</h4>
<p style="text-align: justify;">Para aqueles que trabalham comigo, não é mistério que eu tento ao máximo aplicá-lo aos projetos que participo. Assim, os projetos me geram demandas arquiteturais, que tento contemplá-las no Oragon Architecture, e assim a cada projeto que faço o Oragon Architecture fica maior e mais completo. Eu realmente não faço ideia de quantos projetos já usei a solução, mas com certeza são algumas dezenas. No início o projeto contava apenas com helpers diversos, depois entrou a geração de código estática (ainda usando <a href="http://www.mygenerationsoftware.com/" target="_blank">MyGeneration</a>). Com a geração de código, surgiu a demanda pelos primeiros aspectos AOP, que evoluem sempre: seja com a criação de novos aspectos ou o aperfeiçoamento os aspectos existentes.</p>

<h4 style="text-align: justify;">Services</h4>
<p style="text-align: justify;">O Oragon Architecture foi concebido sobre o Spring.Net, usando e estendendo suas abstrações para prover aplicações mais robustas. A principal feature do Spring.Net que usávamos no início eram os Services Abstractions, que permitiam deploy de serviços já construídos usando diversas tecnologias (<a href="http://www.springframework.net/doc-latest/reference/html/psa-intro.html" target="_blank">leia mais</a>). Essas abstrações permitem facilitar a computação distribuída, hospedando serviços e client-proxies para diversas tecnologias, como:</p>

<ul style="text-align: justify;">
	<li><a href="http://www.springframework.net/doc-latest/reference/html/remoting.html" target="_blank">.NET Remoting</a></li>
	<li><a href="http://www.springframework.net/doc-latest/reference/html/services.html" target="_blank">Enterprise Services</a></li>
	<li><a href="http://www.springframework.net/doc-latest/reference/html/webservices.html" target="_blank">ASMX Web Services</a></li>
	<li><a href="http://www.springframework.net/doc-latest/reference/html/wcf.html" target="_blank">Windows Communication Foundation (WCF)</a></li>
</ul>
<p style="text-align: justify;">Essas abstrações são realizadas via configuração, sem a necessidade de sequer recompilar sua solução. Sob esse princípio, o Oragon Architecture seguiu nessa linha, oferecendo mais e mais abstrações, permitindo que decisões que geralmente envolvem grande refactoring, pudessem ser tomadas a qualquer momento, inclusive após o fim do desenvolvimento do projeto.</p>

<blockquote>A infraestrutura do Spring.Net para abstração de serviços nasceu bem antes do surgimento do WCF. Até sua criação, a substituição de um deploy de serviço embarcado para remoto, não demandava alterações no código.

Somente com a chegada do WCF, e por causa das exigências dos novos atributos (ServiceContract, OperationContract, DataContract, DataMember), esse tipo de troca de abordagem exige pequenas mudanças em seu código.</blockquote>
<h4 style="text-align: justify;">Geração de Código</h4>
<p style="text-align: justify;">Com o amadurecimento do processo de geração de código, por volta de 2010, o MyGeneration foi deixou de ser utilizado como ferramenta auxiliar e incorporei todo o pipeline de geração de código ao projeto, permitindo ainda o versionamento de todas as configurações de geração de código, junto com o projeto. Melhorando a gerência de versões e configuração. Sob a premissa de que código gerado não deve ser modificado manualmente, essa infraestrutura ficou muito robusta na medida que novos recursos como:</p>

<ul style="text-align: justify;">
	<li>Plugins de resolução de padrões de nomenclatura de tabelas e colunas</li>
	<li>Plugins para pluralização em PT-BR e EN-US</li>
	<li>Plugins para Resolução de conflitos de nomes</li>
	<li>E as abstrações para a criação de plugins novos, por demanda, projeto-a-projeto</li>
</ul>
<p style="text-align: justify;">Com esses plugins, é possível mapear uma tabela com nome <strong>TB_USUA</strong> com uma coluna chamada <strong>USUA_CD_USUARIO</strong> em uma classe chamada <strong>Usuario</strong>, com uma propriedade <strong>CodigoUsuario</strong> ou ainda, apenas<strong> Codigo</strong>, com o case correto, inclusive.</p>
<p style="text-align: justify;">As primeiras versões, dos templates, geravam código para NHibernate, e seus HBMs, no entanto com o lançamento do FluentNHibernate, a geração de código ficou mais limpa, como nos exemplos abaixo.</p>

<pre class="lang:c# decode:true">public partial class DirectoryTypeMapper : ClassMap&lt;xxx.DirectoryType&gt;
{
	partial void CompleteMappings();

	public DirectoryTypeMapper()
	{
		Table("[DirectoryType]");
		OptimisticLock.None();
		DynamicUpdate();
		Id(it =&gt; it.IDDirectoryType, "[IDDirectoryType]").GeneratedBy.Assigned();
		HasMany(x =&gt; x.Directories)
			.KeyColumns.Add("[IDDirectoryType]")
			.ForeignKeyConstraintName("[FK_Directory_DirectoryType]")
			.Inverse()
			.Cascade.Delete()				
			.LazyLoad()
			.Fetch.Select()
			.AsBag();
		Map(it =&gt; it.Name, "[Name]").Not.Nullable().CustomSqlType("varchar(300)").Length(300);
		Map(it =&gt; it.Description, "[Description]").Nullable().CustomSqlType("text");
		this.CompleteMappings();
	}		
}</pre>
<p style="text-align: justify;">Nesse exemplo, temos plugins simples, mas coma capacidade e demonstra o plugin de pluralização, seguindo as normas do Inglês (Directories, uma lista de entidades Directory).</p>
<p style="text-align: justify;">Abaixo temos um exemplo mais complexo, excelente para demonstrar as capacidades da Geração de Código Oragon Architecture.</p>

<pre class="lang:c# decode:true">public partial class GrupoMapper : ClassMap&lt;xxx.Grupo&gt;
{
	partial void CompleteMappings();

	public GrupoMapper()
	{
		Table("[GrupoAcesso]");
		Id(it =&gt; it.CodGrupo, "CodGrupoAcesso").GeneratedBy.Identity();
		HasManyToMany(x =&gt; x.Acoes)
			.ParentKeyColumns.Add("[CodGrupoAcesso]")
			.Table("[PermissaoGrupoAcesso]")
			.ChildKeyColumns.Add("[CodAcaoSistema]");
		HasMany(x =&gt; x.ProfissionaisGrupo)
			.KeyColumns.Add("[CodGrupoAcesso]")
			.ForeignKeyConstraintName("FK_PRGA_GRAC");
		Map(it =&gt; it.Nome, "NomGrupoAcesso").Not.Nullable().CustomSqlType("varchar(100)").Length(100);
		Map(it =&gt; it.GrupoAdministrativo, "FlgGrupoAdministrativo").Not.Nullable().CustomSqlType("bit");
		this.CompleteMappings();
	}		
}

public partial class Grupo
{
	public virtual IList&lt;Acao&gt; Acoes { get; set; }
	public virtual IList&lt;ProfissionalGrupo&gt; ProfissionaisGrupo { get; set; }
	public virtual short CodGrupo { get; set; }
	public virtual string Nome { get; set; }
	public virtual bool GrupoAdministrativo { get; set; }
}
</pre>
<p style="text-align: justify;">Nesse exemplo, temos algumas features bem interessantes:</p>

<ul style="text-align: justify;">
	<li>Suporte a convenções onde a propriedade CodGrupoAcesso é gera uma propriedade chamada CodGrupo.</li>
	<li>As tabelas <strong>GrupoAcesso</strong> e <strong>AcaoSistema</strong> geram, respectivamente, as entidades <strong>Grupo</strong> e <strong>Acao</strong>.</li>
	<li>As pluralizações corretas
<ul>
	<li>IList&lt;Ac<span style="color: #ff0000;">ao</span>&gt; Ac<span style="color: #ff0000;">oes</span></li>
	<li>IList&lt;Profission<span style="color: #ff0000;">al</span>Grup<span style="color: #ff0000;">o</span>&gt; Profissiona<span style="color: #ff0000;">is</span>Grup<span style="color: #ff0000;">o</span></li>
</ul>
</li>
	<li>A resolução de convenções de criação de banco como a coluna FlgGrupoAdministrativo gerando a propriedade GrupoAdministrativo.</li>
</ul>
<blockquote><span style="color: #ff9900;"><em>OS EXEMPLOS NÃO FORAM MANIPULADOS MANUALMENTE, são exemplos de código gerado automaticamente.</em></span>


<span style="color: #ff9900;"><em>O gerador, quando configurado corretamente, tem a capacidade de realizar conversões de nomes de colunas e tabelas e resolução de plurais. Tudo para gerar um código mais limpo e o mais próximo do que modelaríamos manualmente, se o fizéssemos.</em></span></blockquote>
<p style="text-align: justify;">A infraestrutura de geração de código foi criada pensando no GAP entre DBAs e suas regras e os desenvolvedores. É comum, encontrarmos um conjunto de regras absurdo, em grandes corporações. `As técnicas e features usadas no gerador visam assimilar as regras de conversão para tornar sua vida mais produtiva, e independente.`</p>

<h3 style="text-align: justify;"><a href="http://luizcarlosfaria.net/category/oragon-architecture/">Leia mais sobre Oragon Architecture!</a></h3>
