﻿using CardanoSharp.Blazor.Components.Enums;
using CardanoSharp.Blazor.Components.Exceptions;
using CardanoSharp.Blazor.Components.Models.Errors;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Extensions
{
    public static class JSExceptionExtension
    {
        public static WebWalletException ToWebWalletException(this JSException jsException, WebWalletErrorType errorTypeHint = WebWalletErrorType.Unknown)
        {
            try
            {
                var errorCode = JsonConvert.DeserializeObject<InfoCodeError>(jsException.Message);
                if (errorCode != null && errorCode.code != 0)
                {
                    if (errorCode.code > 0)
                    {
                        //only the more method specific errors have positive codes
                        //either datasign/txsend/txsign
                        switch (errorTypeHint)
                        {
                            case WebWalletErrorType.DataSign:
                                return new DataSignException(errorCode, "", jsException);
                            case WebWalletErrorType.TxSend:
                                return new TxSendException(errorCode, "", jsException);
                            case WebWalletErrorType.TxSign:
                                return new TxSignException(errorCode, "", jsException);
                        }

                    }

                    //is definitely error code type error (API Error in CIP30 spec)
                    return new ErrorCodeException(errorCode, "Web wallet api error", jsException);
                }
            }
            catch { }

            try
            {
                var paginateError = JsonConvert.DeserializeObject<PaginateError>(jsException.Message);
                if (paginateError != null && paginateError.maxSize != 0)
                {
                    //is definitely paginate type error
                    return new PaginateException(paginateError, "Web wallet paginate error", jsException);
                }
            }
            catch { }

            //cant determine error type, return general error
            return new WebWalletException("Unknown web wallet exception", jsException);
        }
    }
}
