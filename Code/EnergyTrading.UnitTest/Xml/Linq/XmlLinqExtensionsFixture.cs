namespace EnergyTrading.UnitTest.Xml.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.XPath;

    using EnergyTrading.Logging;
    using EnergyTrading.Xml;
    using EnergyTrading.Xml.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class XmlLinqExtensionsFixture
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string TradeXml = @"<trade:TradePayload xsi:schemaLocation=""http://rwe.com/schema/trade/2 ../../../RwestTrade.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:trade=""http://rwe.com/schema/trade/2"" xmlns:party=""http://rwe.com/schema/party/2"" xmlns:common=""http://rwe.com/schema/common/3"" xmlns:product=""http://rwe.com/schema/product/2"">
  <SchemaRelease xmlns = ""http://rwe.com/schema/messaging/2"" >
    <Version> 2.1 </Version>
    <Notes>TradeSchema XSD Development Version 2.1.599</Notes>
  </SchemaRelease>
  <trade:Trade>
    <common:Id>
      <common:SystemId>
        <common:SystemID>Trayport</common:SystemID>
        <common:Identifier>2118143</common:Identifier>
        <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
      </common:SystemId>
    </common:Id>
    <trade:TradeData>
      <trade:TradeTimestamp>2011-11-09T08:10:15Z</trade:TradeTimestamp>
      <trade:TradeStatus>Executed</trade:TradeStatus>
      <trade:TradeDirection>Sell</trade:TradeDirection>
      <trade:InternalParty>
        <trade:PartyRole>Aggressor</trade:PartyRole>
        <trade:LegalEntity>
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>892</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <party:Details>
            <party:ShortName>RWE TRADING</party:ShortName>
            <party:LongName>RWE TRADING</party:LongName>
            <party:PartyType>Counterparty</party:PartyType>
          </party:Details>
        </trade:LegalEntity>
        <trade:BusinessUnit>
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>203</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>Trayport</common:SystemID>
              <common:Identifier>RWE Supply &amp; Trading GmbH</common:Identifier>
              <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <party:Details>
            <party:ShortName>RWE TRADING BU</party:ShortName>
            <party:LongName>RWE TRADING BU</party:LongName>
            <party:PartyType>Counterparty</party:PartyType>
          </party:Details>
        </trade:BusinessUnit>
        <trade:Trader>
          <common:Id>
            <common:SystemId>
              <common:SystemID>Trayport</common:SystemID>
              <common:Identifier>Ben Crawforth</common:Identifier>
              <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>2778</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>ADC</common:SystemID>
              <common:Identifier>195</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>Common</common:SystemID>
              <common:Identifier>GROUP\RE51675</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>Endur</common:SystemID>
              <common:Identifier>RE51675</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>Spreadsheet</common:SystemID>
              <common:Identifier>133</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>CommercialDesktop:Gas UK</common:SystemID>
              <common:Identifier>GROUP\RE51675</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>CommercialDesktop:Gas|Gas UK</common:SystemID>
              <common:Identifier>GROUP\RE51675</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <common:Details>
            <common:Name>Ben Crawforth</common:Name>
            <common:Forename>Ben</common:Forename>
            <common:Surname>Crawforth</common:Surname>
            <common:TelephoneNumber></common:TelephoneNumber>
            <common:FaxNumber></common:FaxNumber>
            <common:Role>Trader</common:Role>
          </common:Details>
        </trade:Trader>
      </trade:InternalParty>
      <trade:ExternalParty>
        <trade:PartyRole>Initiator</trade:PartyRole>
        <trade:LegalEntity>
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>885</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <party:Details>
            <party:ShortName>24/7 TRADING</party:ShortName>
            <party:LongName>24/7 TRADING</party:LongName>
            <party:PartyType>Counterparty</party:PartyType>
          </party:Details>
        </trade:LegalEntity>
        <trade:BusinessUnit>
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>8</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
            <common:SystemId>
              <common:SystemID>Trayport</common:SystemID>
              <common:Identifier>24/7 Trading Gmbh</common:Identifier>
              <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <party:Details>
            <party:ShortName>24/7 TRADING BU</party:ShortName>
            <party:LongName>24/7 TRADING BU</party:LongName>
            <party:PartyType>Counterparty</party:PartyType>
          </party:Details>
        </trade:BusinessUnit>
        <trade:Trader>
          <common:Id>
            <common:SystemId>
              <common:SystemID>Trayport</common:SystemID>
              <common:Identifier>Amit Gudka</common:Identifier>
              <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <common:Details>
            <common:Name>Amit Gudka</common:Name>
          </common:Details>
        </trade:Trader>
      </trade:ExternalParty>
      <trade:AgentParties>
        <trade:TradeAgentParty>
          <trade:Role>Broker</trade:Role>
          <trade:AgentParty>
            <common:Id>
              <common:SystemId>
                <common:SystemID>Trayport</common:SystemID>
                <common:Identifier>PREBON_RWE1</common:Identifier>
                <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
              </common:SystemId>
            </common:Id>
            <!-- NOTE: Once provided by MDM, this needs to add the EnergyTrading ID of the broker party -->
            <party:Details>
              <party:ShortName>1PREBON BU</party:ShortName>
              <party:LongName>1PREBON BU</party:LongName>
              <party:PartyType>Broker</party:PartyType>
            </party:Details>
          </trade:AgentParty>
          <trade:Fee>
            <common:FeeRate>0.003000</common:FeeRate>
            <common:OverrideFlag>false</common:OverrideFlag>
          </trade:Fee>
        </trade:TradeAgentParty>
      </trade:AgentParties>
      <trade:TimePeriod>
        <common:Start>2012-01-01</common:Start>
        <common:End>2012-03-31</common:End>
      </trade:TimePeriod>
      <trade:AdditionalProperties> 
        <common:Property> 
          <common:Name>ADC Commodity</common:Name> 
          <common:Value>GasPhysical</common:Value>
        </common:Property> 
      </trade:AdditionalProperties>
    </trade:TradeData>
    <trade:TradeProductData>
      <trade:Product>
        <common:Id>
          <common:SystemId>
            <common:SystemID>Trayport</common:SystemID>
            <common:Identifier>10002071_10000306_NBP_2012 Q1</common:Identifier>
            <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
          </common:SystemId>
          <common:SystemId>
            <common:SystemID>EnergyTrading</common:SystemID>
            <common:Identifier>PTI/5217</common:Identifier>
            <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
          </common:SystemId>
        </common:Id>
        <product:Details>
          <product:HierarchyLevel>ProductTypeInstance</product:HierarchyLevel>
          <product:Name>NBP 2012 Q1</product:Name>
          <product:Commodity>
            <common:Id>
              <common:SystemId>
                <common:SystemID>EnergyTrading</common:SystemID>
                <common:Identifier>15</common:Identifier>
                <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
              </common:SystemId>
            </common:Id>
            <product:Details>
              <product:Name>Natural Gas Hi Cal</product:Name>
            </product:Details>
          </product:Commodity>
          <product:Location xsi:type= ""common:PhysicalDeliveryLocationType"" >
            <common:Id>
              <common:SystemId>
                <common:SystemID>EnergyTrading</common:SystemID>
                <common:Identifier>65</common:Identifier>
                <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
              </common:SystemId>
            </common:Id>
			<!-- Explicit type reference in following is not necessary, but also not wrong -->
            <common:Details xsi:type= ""common:PhysicalDeliveryLocationDetailsType"" >
              <common:Name>NBP</common:Name>
            </common:Details>
          </product:Location>
          <product:Market>
            <common:Id>
              <common:SystemId>
                <common:SystemID>EnergyTrading</common:SystemID>
                <common:Identifier>25</common:Identifier>
                <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
              </common:SystemId>
            </common:Id>
            <product:Details>
              <product:Name>NBP</product:Name>
              <product:Commodity>
                <common:Id>
                  <common:SystemId>
                    <common:SystemID>EnergyTrading</common:SystemID>
                    <common:Identifier>15</common:Identifier>
                    <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                  </common:SystemId>
                </common:Id>
                <product:Details>
                  <product:Name>Natural Gas Hi Cal</product:Name>
                </product:Details>
              </product:Commodity>
              <product:Calendar>
                <common:Id>
                  <common:SystemId>
                    <common:SystemID>EnergyTrading</common:SystemID>
                    <common:Identifier>36</common:Identifier>
                    <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                  </common:SystemId>
                </common:Id>
                <product:Details>
                  <product:Name>UK Calendar</product:Name>
                </product:Details>
              </product:Calendar>
			  <product:Location xsi:type= ""common:PhysicalDeliveryLocationType"" >

                <common:Id>
				  <common:SystemId>
					<common:SystemID>EnergyTrading</common:SystemID>
					<common:Identifier>65</common:Identifier>
					<common:OriginatingSourceIND>false</common:OriginatingSourceIND>
				  </common:SystemId>
				</common:Id>
				<!-- Explicit type reference in following is not necessary, but also not wrong -->
				<common:Details xsi:type= ""common:PhysicalDeliveryLocationDetailsType"" >

                  <common:Name>NBP</common:Name>
				</common:Details>
			  </product:Location>
              <product:Currency>GBX</product:Currency>
              <product:TradeUnitOfMeasure>kth</product:TradeUnitOfMeasure>
              <product:TradeUnitsRate>Day</product:TradeUnitsRate>
              <product:NominationUnitOfMeasure>MWh</product:NominationUnitOfMeasure>
              <product:PriceUnitOfMeasure>th</product:PriceUnitOfMeasure>
            </product:Details>
          </product:Market>
          <product:DeliveryPeriod>2012 Q1</product:DeliveryPeriod>
          <product:Currency>GBX</product:Currency>
          <product:UnitOfMeasure>kth/d</product:UnitOfMeasure>
          <product:MultiplierData>
            <product:TradedPriceMultiplier>1</product:TradedPriceMultiplier>
            <product:TradedVolumeMultiplier>1</product:TradedVolumeMultiplier>
          </product:MultiplierData>
        </product:Details>
      </trade:Product>
      <trade:TradedQuantity>25</trade:TradedQuantity>
      <trade:TradedPrice>66.8</trade:TradedPrice>
    </trade:TradeProductData>
    <trade:InstrumentData>
      <trade:InstrumentType>Forward</trade:InstrumentType>
    </trade:InstrumentData>
    <trade:Legs>
	  <!-- Explicit namespace reference in following is not necessary, but also not wrong -->
      <trade:Leg xmlns:trade=""http://rwe.com/schema/trade/2"" xsi:type=""trade:FixedFinancialLegType"">
        <trade:Id>
          <common:SystemId>
            <common:SystemID>EnergyTrading</common:SystemID>
            <common:Identifier>pricing</common:Identifier>
            <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
          </common:SystemId>
        </trade:Id>
        <trade:TimePeriod>
          <common:Start>2012-01-01</common:Start>
          <common:End>2012-03-31</common:End>
        </trade:TimePeriod>
        <trade:LegDirection>Take</trade:LegDirection>
        <trade:Currency>GBX</trade:Currency>
        <trade:PriceUnitOfMeasure>th</trade:PriceUnitOfMeasure>
        <trade:QuantityRate>
          <trade:UnitOfMeasure>kth</trade:UnitOfMeasure>
          <trade:RateInterval>d</trade:RateInterval>
          <trade:Rate>25</trade:Rate>
        </trade:QuantityRate>
        <trade:TotalQuantity>2275</trade:TotalQuantity>
        <trade:QuantityUnitOfMeasure>kth</trade:QuantityUnitOfMeasure>
        <trade:TotalAmount>151970000</trade:TotalAmount>
        <trade:LegPeriods>
          <trade:LegPeriod>
            <trade:LegPeriodID>0</trade:LegPeriodID>
            <trade:TimePeriod>
              <common:Start>2012-01-01</common:Start>
              <common:End>2012-03-31</common:End>
            </trade:TimePeriod>
            <trade:QuantityRate>25</trade:QuantityRate>
            <trade:Quantity>2275</trade:Quantity>
            <trade:UnitPrice>66.8</trade:UnitPrice>
            <trade:TotalAmount>151970000</trade:TotalAmount>
          </trade:LegPeriod>
        </trade:LegPeriods>
      </trade:Leg>
	  <!-- Explicit namespace reference in following is not necessary, but also not wrong -->
      <trade:Leg xmlns:trade=""http://rwe.com/schema/trade/2"" xsi:type=""trade:PhysicalDeliveryLegType"">
        <trade:Id>
          <common:SystemId>
            <common:SystemID>EnergyTrading</common:SystemID>
            <common:Identifier>product</common:Identifier>
            <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
          </common:SystemId>
        </trade:Id>
        <trade:TimePeriod>
          <common:Start>2012-01-01</common:Start>
          <common:End>2012-03-31</common:End>
        </trade:TimePeriod>
        <trade:LegDirection>Give</trade:LegDirection>
		<trade:Product>
		  <common:Id>
			<common:SystemId>
			  <common:SystemID>Trayport</common:SystemID>
			  <common:Identifier>10002071_10000306_NBP_2012 Q1</common:Identifier>
			  <common:OriginatingSourceIND>true</common:OriginatingSourceIND>
			</common:SystemId>
			<common:SystemId>
			  <common:SystemID>EnergyTrading</common:SystemID>
			  <common:Identifier>PTI/5217</common:Identifier>
			  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
			</common:SystemId>
		  </common:Id>
		  <product:Details>
			<product:HierarchyLevel>ProductTypeInstance</product:HierarchyLevel>
			<product:Name>NBP 2012 Q1</product:Name>
            <product:Commodity>
              <common:Id>
                <common:SystemId>
                  <common:SystemID>EnergyTrading</common:SystemID>
                  <common:Identifier>15</common:Identifier>
                  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                </common:SystemId>
              </common:Id>
              <product:Details>
                <product:Name>Natural Gas Hi Cal</product:Name>
              </product:Details>
            </product:Commodity>
            <product:Location xsi:type= ""common:PhysicalDeliveryLocationType"" >
              <common:Id>
                <common:SystemId>
                  <common:SystemID>EnergyTrading</common:SystemID>
                  <common:Identifier>65</common:Identifier>
                  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                </common:SystemId>
              </common:Id>
              <common:Details xsi:type= ""common:PhysicalDeliveryLocationDetailsType"" >
                <common:Name>NBP</common:Name>
              </common:Details>
            </product:Location>
            <product:Market>
              <common:Id>
                <common:SystemId>
                  <common:SystemID>EnergyTrading</common:SystemID>
                  <common:Identifier>25</common:Identifier>
                  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                </common:SystemId>
              </common:Id>
              <product:Details>
                <product:Name>NBP</product:Name>
                <product:Commodity>
                  <common:Id>
                    <common:SystemId>
                      <common:SystemID>EnergyTrading</common:SystemID>
                      <common:Identifier>15</common:Identifier>
                      <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                    </common:SystemId>
                  </common:Id>
                  <product:Details>
                    <product:Name>Natural Gas Hi Cal</product:Name>
                  </product:Details>
                </product:Commodity>
                <product:Calendar>
                  <common:Id>
                    <common:SystemId>
                      <common:SystemID>EnergyTrading</common:SystemID>
                      <common:Identifier>36</common:Identifier>
                      <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                    </common:SystemId>
                  </common:Id>
                  <product:Details>
                    <product:Name>UK Calendar</product:Name>
                  </product:Details>
                </product:Calendar>
				<product:Location xsi:type= ""common:PhysicalDeliveryLocationType"" >

                    <common:Id>
					  <common:SystemId>
						<common:SystemID>EnergyTrading</common:SystemID>
						<common:Identifier>65</common:Identifier>
						<common:OriginatingSourceIND>false</common:OriginatingSourceIND>
					  </common:SystemId>
					</common:Id>
					<!-- Explicit type reference in following is not necessary, but also not wrong -->
					<common:Details xsi:type= ""common:PhysicalDeliveryLocationDetailsType"" >

                      <common:Name>NBP</common:Name>
					</common:Details>
				</product:Location>
                <product:Currency>GBX</product:Currency>
                <product:TradeUnitOfMeasure>kth</product:TradeUnitOfMeasure>
                <product:TradeUnitsRate>Day</product:TradeUnitsRate>
                <product:NominationUnitOfMeasure>MWh</product:NominationUnitOfMeasure>
                <product:PriceUnitOfMeasure>th</product:PriceUnitOfMeasure>
              </product:Details>
            </product:Market>
            <product:DeliveryPeriod>2012 Q1</product:DeliveryPeriod>
            <product:Currency>GBX</product:Currency>
            <product:UnitOfMeasure>kth/d</product:UnitOfMeasure>
            <product:MultiplierData>
              <product:TradedPriceMultiplier>1</product:TradedPriceMultiplier>
              <product:TradedVolumeMultiplier>1</product:TradedVolumeMultiplier>
            </product:MultiplierData>
          </product:Details>
        </trade:Product>
        <trade:Market>
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>25</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <product:Details>
            <product:Name>NBP</product:Name>
            <product:Commodity>
              <common:Id>
                <common:SystemId>
                  <common:SystemID>EnergyTrading</common:SystemID>
                  <common:Identifier>15</common:Identifier>
                  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                </common:SystemId>
              </common:Id>
              <product:Details>
                <product:Name>Natural Gas Hi Cal</product:Name>
              </product:Details>
            </product:Commodity>
            <product:Calendar>
              <common:Id>
                <common:SystemId>
                  <common:SystemID>EnergyTrading</common:SystemID>
                  <common:Identifier>36</common:Identifier>
                  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                </common:SystemId>
              </common:Id>
              <product:Details>
                <product:Name>UK Calendar</product:Name>
              </product:Details>
            </product:Calendar>
            <product:Location xsi:type= ""common:PhysicalDeliveryLocationType"" >
              <common:Id>
                <common:SystemId>
                  <common:SystemID>EnergyTrading</common:SystemID>
                  <common:Identifier>65</common:Identifier>
                  <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
                </common:SystemId>
              </common:Id>
			  <!-- Explicit type reference in following is not necessary, but also not wrong -->
              <common:Details xsi:type= ""common:PhysicalDeliveryLocationDetailsType"" >
                <common:Name>NBP</common:Name>
              </common:Details>
            </product:Location>
            <product:Currency>GBX</product:Currency>
            <product:TradeUnitOfMeasure>kth</product:TradeUnitOfMeasure>
            <product:TradeUnitsRate>Day</product:TradeUnitsRate>
            <product:NominationUnitOfMeasure>MWh</product:NominationUnitOfMeasure>
            <product:PriceUnitOfMeasure>th</product:PriceUnitOfMeasure>
          </product:Details>
        </trade:Market>
        <trade:Commodity>
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>15</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
          <product:Details>
            <product:Name>Natural Gas Hi Cal</product:Name>
          </product:Details>
        </trade:Commodity>
        <trade:Location xsi:type= ""common:PhysicalDeliveryLocationType"" >
          <common:Id>
            <common:SystemId>
              <common:SystemID>EnergyTrading</common:SystemID>
              <common:Identifier>65</common:Identifier>
              <common:OriginatingSourceIND>false</common:OriginatingSourceIND>
            </common:SystemId>
          </common:Id>
			<!-- Explicit type reference in following is not necessary, but also not wrong -->
            <common:Details xsi:type= ""common:PhysicalDeliveryLocationDetailsType"" >
            <common:Name>NBP</common:Name>
          </common:Details>
        </trade:Location>
        <trade:QuantityRate>
          <trade:UnitOfMeasure>kth</trade:UnitOfMeasure>
          <trade:RateInterval>d</trade:RateInterval>
          <trade:Rate>25</trade:Rate>
        </trade:QuantityRate>
        <trade:TotalQuantity>2275</trade:TotalQuantity>
        <trade:QuantityUnitOfMeasure>kth</trade:QuantityUnitOfMeasure>
        <trade:LegPeriods>
          <trade:LegPeriod>
            <trade:LegPeriodID>0</trade:LegPeriodID>
            <trade:TimePeriod>
              <common:Start>2012-01-01</common:Start>
              <common:End>2012-03-31</common:End>
            </trade:TimePeriod>
            <trade:QuantityRate>25</trade:QuantityRate>
            <trade:Quantities>
              <trade:Quantity>
                <trade:Value>2275</trade:Value>
                <trade:Type>Traded</trade:Type>
              </trade:Quantity>
            </trade:Quantities>
            <!-- NOTE: Once MDM is set up and/or there are other requirements for the exact delivery period, this should be filled
            <trade:FlatDeliveryData>
				<common:Start>2012-01-01T06:00:00Z</common:Start>
				<common:End>2012-04-01T06:00:00Z</common:End>
			</trade:FlatDeliveryData>
			-->
          </trade:LegPeriod>
        </trade:LegPeriods>
      </trade:Leg>
    </trade:Legs>
  </trade:Trade>
</trade:TradePayload>";

        private const string Xml = @"<Fred xmlns='http://sample.com' xmlns:a='http://sample.com/a'>
                                        <Jim xmlns='http://test.com'>a</Jim>
                                        <Bob>b</Bob>
                                     </Fred>";

        [Test]
        public void XDocumentNamespaceFile()
        {
            var document = XDocument.Parse(TradeXml);

            this.TimeIt(document.Namespaces().ToList);
        }

        [Test]
        public void XDocumentNamespaceXml()
        {
            var document = XDocument.Parse(Xml);

            this.TimeIt(document.Namespaces().ToList);
        }

        [Test]
        public void XPathDocumentNamespaceFile()
        {
            var document = new XPathDocument(new StringReader(TradeXml));

            this.TimeIt(document.Namespaces().ToList);
        }

        [Test]
        public void XPathDocumentNamespaceXml()
        {
            var document = new XPathDocument(new StringReader(Xml));

            this.TimeIt(document.Namespaces().ToList);
        }

        [Test]
        public void XPathDocumenNamespace2File()
        {
            var document = new XPathDocument(new StringReader(TradeXml));

            this.TimeIt(document.Namespaces2().ToList);
        }

        [Test]
        public void XPathDocumenNamespace2Xml()
        {
            var document = new XPathDocument(new StringReader(Xml));

            this.TimeIt(document.Namespaces2().ToList);
        }

        private void TimeIt(Func<List<Tuple<string, string>>> f)
        {
            var w = new Stopwatch();
            w.Start();

            var d = f();
            for (var i = 0; i <1000; i++)
            {
                d = f();
            }

            w.Stop();

            Logger.InfoFormat("Found {0} in {1} ms", d.Count, w.ElapsedMilliseconds);
        }

        [Test]
        public void ShouldNormalizeWithoutSchema()
        {
            var input = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                                <Child>1</Child>
                                            </Root>");

            var afterNormalize = input.Normalize(null);

            Assert.IsNotNull(afterNormalize);
        }

        [Test]
        public void ShouldNotChangeTheContentOfNormalizeXml()
        {
            var input = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                                <Child>1</Child>
                                            </Root>");

            var afterNormalize = input.Normalize(null);

            Assert.IsNotNull(afterNormalize);
            Assert.IsTrue(afterNormalize.DeepEqualsWithNormalization(input, null));
        }

        [Test]
        public void ShouldReturnTrueForTwoSemanticallyEquivalentOrIsometricXmls()
        {
            var doc1 = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                            <Child>1</Child>
                                        </Root>");

            var doc2 = XDocument.Parse(@"<n:Root xmlns:n='http://www.northwind.com'>
                                            <n:Child>1</n:Child>
                                        </n:Root>");

            Assert.IsTrue(doc1.DeepEqualsWithNormalization(doc2, null));
        }

        [Test]
        public void ShouldReturnTrueIfTwoSemanticallyEquivalentOrIsometricXmlsHasDifferentOrderOfAttributes()
        {
            var doc1 = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                            <Child a='1' b='2'>1</Child>
                                        </Root>");

            var doc2 = XDocument.Parse(@"<n:Root xmlns:n='http://www.northwind.com'>
                                            <n:Child  b='2' a='1'>1</n:Child>
                                        </n:Root>");

            Assert.IsTrue(doc1.DeepEqualsWithNormalization(doc2, null));
        }

        [Test]
        public void ShouldNormalizeWithSchema()
        {
            var doc = XDocument.Parse(@"<Root xmlns:n='http://www.northwind.com'>
                                            <Child1>abc</Child1>
                                            <Child2>xyz</Child2>
                                        </Root>");

            var afterNormaize = doc.Normalize(this.SchemaSet);
            Assert.IsNotNull(afterNormaize);
        }

        [Test]
        public void ShouldReturnTrueForTwoSemanticallyEquivalentOrIsometricXmlsWithSchemaValidation()
        {
            var doc1 = XDocument.Parse(@"<Root xmlns='http://www.northwind.com'>
                                            <Child1>1</Child1>
                                            <Child2>1</Child2>
                                        </Root>");

            var doc2 = XDocument.Parse(@"<n:Root xmlns:n='http://www.northwind.com'>
                                            <n:Child1>1</n:Child1>
                                            <n:Child2>1</n:Child2>
                                        </n:Root>");


            Assert.IsTrue(doc1.DeepEqualsWithNormalization(doc2, this.SchemaSet));
        }

        [Test]
        public void ToXElementBool()
        {
            var value = true;

            var candidate = value.ToXElement("Test");

            Assert.AreEqual("true", candidate.Value);
        }

        [Test]
        public void ToXElementBoolDefaultValue()
        {
            var value = false;

            var candidate = value.ToXElement("Test");

            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXElementBoolOutputDefaultValue()
        {
            var value = false;

            var candidate = value.ToXElement("Test", outputDefault: true);

            Assert.AreEqual("false", candidate.Value); 
        }

        [Test]
        public void ToXElementInt()
        {
            var value = 1;

            var candidate = value.ToXElement("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [Test]
        public void ToXElementIntDefaultValue()
        {
            var value = int.MinValue;

            var candidate = value.ToXElement("Test");

            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXElementIntOutputDefaultValue()
        {
            var value = 3;

            var candidate = value.ToXElement("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [Test]
        public void ToXElementDecimal()
        {
            decimal value = 1;

            var candidate = value.ToXElement("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [Test]
        public void ToXElementDecimaltDefaultValue()
        {
            decimal value = decimal.MinValue;

            var candidate = value.ToXElement("Test");

            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXElementDecimalOutputDefaultValue()
        {
            decimal value = 3;

            var candidate = value.ToXElement("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [Test]
        public void ToXElementDateTime()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXElement("Test");
            Assert.AreEqual("2012-05-13T23:14:34Z", candidate.Value);
        }

        [Test]
        public void ToXElementDateTimeWithFormat()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXElement("Test", format: XmlExtensions.DateFormat);
            Assert.AreEqual("2012-05-13", candidate.Value);
        }

        [Test]
        public void ToXAttributeBool()
        {
            var value = true;

            var candidate = value.ToXAttribute("Test");

            Assert.AreEqual("true", candidate.Value);
        }

        [Test]
        public void ToXAttributeBoolDefaultValue()
        {
            var value = false;

            var candidate = value.ToXAttribute("Test");

            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXAttributeBoolOutputDefaultValue()
        {
            var value = false;

            var candidate = value.ToXAttribute("Test", outputDefault: true);

            Assert.AreEqual("false", candidate.Value);
        }

        [Test]
        public void ToXAttributeInt()
        {
            var value = 1;

            var candidate = value.ToXAttribute("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [Test]
        public void ToXAttributeIntDefaultValue()
        {
            var value = int.MinValue;

            var candidate = value.ToXAttribute("Test");

            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXAttributeIntOutputDefaultValue()
        {
            var value = 3;

            var candidate = value.ToXAttribute("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [Test]
        public void ToXAttributeDecimal()
        {
            decimal value = 1;

            var candidate = value.ToXAttribute("Test");

            Assert.AreEqual("1", candidate.Value);
        }

        [Test]
        public void ToXAttributeDecimaltDefaultValue()
        {
            decimal value = decimal.MinValue;

            var candidate = value.ToXAttribute("Test");

            Assert.IsNull(candidate);
        }

        [Test]
        public void ToXAttributeDecimalOutputDefaultValue()
        {
            decimal value = 3;

            var candidate = value.ToXAttribute("Test", outputDefault: true, defaultValue: 3);

            Assert.AreEqual("3", candidate.Value);
        }

        [Test]
        public void ToXAttributeDateTime()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXAttribute("Test");
            Assert.AreEqual("2012-05-13T23:14:34Z", candidate.Value);
        }

        [Test]
        public void ToXAttributeDateTimeWithFormat()
        {
            var value = new DateTime(2012, 5, 13, 23, 14, 34);

            var candidate = value.ToXAttribute("Test", format: XmlExtensions.DateFormat);
            Assert.AreEqual("2012-05-13", candidate.Value);
        }

        [Test]
        public void GetChildElementValueReturnsNullForNullElement()
        {
            var candidate = XmlLinqExtensions.GetChildElementValue(null, "test");
            Assert.IsNull(candidate);
        }

        [Test]
        public void GetChildElementValueReturnsNullIfNoChildElements()
        {
            var candidate = new XElement("element").GetChildElementValue("test");
            Assert.IsNull(candidate);
        }

        [Test]
        public void GetChildElementValueReturnsValueIfPresent()
        {
            var candidate = new XElement("element", new XElement("test", "value")).GetChildElementValue("test");
            Assert.IsNotNull(candidate);
            Assert.AreEqual("value", candidate);
        }

        private XmlSchemaSet SchemaSet
        {
            get
            {
                var xsdMarkup =
                    @"<xsd:schema attributeFormDefault='unqualified' elementFormDefault='qualified' version='1.0' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                      <xsd:element name='Root'>
                        <xsd:complexType>
                          <xsd:sequence>
                            <xsd:element name='Child1' type='xsd:string' />
                            <xsd:element name='Child2' type='xsd:string' />
                          </xsd:sequence>
                        </xsd:complexType>
                      </xsd:element>
                    </xsd:schema>";
                var schemas = new XmlSchemaSet();
                schemas.Add(@"http://www.northwind.com", XmlReader.Create(new StringReader(xsdMarkup)));

                return schemas;
            }
        }
    }
}