//Este código de Template foi gerado com IA, como inspiração de template, o site https://inoa.com/

public static class EmailTemplate
{
    public static string BuildAlert(
        string title,
        string action,
        string color,
        Stock stock,
        decimal currentPrice,
        decimal referencePrice,
        DateTime date)
    {
         return $"""
        <div style="
            font-family: Arial, Helvetica, sans-serif;
            background-color: #F5F7F8;
            padding: 32px;
            color: #1F2933;
        ">

            <div style="
                max-width: 560px;
                margin: 0 auto;
                background-color: #FFFFFF;
                border-radius: 10px;
                overflow: hidden;
                border: 1px solid #E6E9EC;
            ">

                <div style="
                    background-color: #111820;
                    padding: 20px 24px;
                ">
                    <span style="
                        color: #FFFFFF;
                        font-size: 20px;
                        font-weight: bold;
                        letter-spacing: 1px;
                    ">
                        STOCK QUOTE ALERT
                    </span>
                </div>

                <div style="padding: 28px 24px;">

                    <div style="
                        display: inline-block;
                        background-color: {color};
                        color: #FFFFFF;
                        padding: 6px 12px;
                        border-radius: 4px;
                        font-size: 12px;
                        font-weight: bold;
                    ">
                        {action}
                    </div>

                    <h2 style="
                        margin: 16px 0 8px 0;
                        font-size: 24px;
                        color: #111820;
                    ">
                        {title}
                    </h2>

                    <p style="
                        margin: 0 0 24px 0;
                        color: #66717C;
                        font-size: 14px;
                    ">
                        O ativo <strong>{stock.Symbol}</strong> atingiu o limite configurado.
                    </p>

                    <div style="
                        background-color: #F8FAFB;
                        border-left: 4px solid {color};
                        padding: 18px;
                        margin-bottom: 20px;
                    ">
                        <div style="
                            font-size: 13px;
                            color: #7A858F;
                            margin-bottom: 4px;
                        ">
                            Cotação atual
                        </div>

                        <div style="
                            font-size: 28px;
                            font-weight: bold;
                            color: #111820;
                        ">
                            R$ {currentPrice:F2}
                        </div>
                    </div>

                    <table style="
                        width: 100%;
                        border-collapse: collapse;
                        font-size: 14px;
                    ">
                        <tr>
                            <td style="
                                padding: 10px 0;
                                color: #7A858F;
                                border-bottom: 1px solid #ECEFF1;
                            ">
                                Ativo
                            </td>

                            <td style="
                                text-align: right;
                                font-weight: bold;
                                border-bottom: 1px solid #ECEFF1;
                            ">
                                {stock.Symbol}
                            </td>
                        </tr>

                        <tr>
                            <td style="
                                padding: 10px 0;
                                color: #7A858F;
                                border-bottom: 1px solid #ECEFF1;
                            ">
                                Preço de referência
                            </td>

                            <td style="
                                text-align: right;
                                font-weight: bold;
                                border-bottom: 1px solid #ECEFF1;
                            ">
                                R$ {referencePrice:F2}
                            </td>
                        </tr>

                        <tr>
                            <td style="
                                padding: 10px 0;
                                color: #7A858F;
                            ">
                                Horário da cotação
                            </td>

                            <td style="
                                text-align: right;
                                font-weight: bold;
                            ">
                                {date:dd/MM/yyyy HH:mm:ss}
                            </td>
                        </tr>
                    </table>

                </div>

                <div style="
                    padding: 16px 24px;
                    background-color: #F8FAFB;
                    color: #8A949D;
                    font-size: 12px;
                    text-align: center;
                ">
                    Monitoramento automático de cotação
                </div>

            </div>
        </div>
        """;
    }
}