import * as fromAccount from './account/account-reducers';

export interface State {
    account: fromAccount.State
}

export const reducers = {
    account: fromAccount.reducer
}