import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';


export interface PredictionResult {
    results: number[];
    predictedValue: number;
    success: boolean;
    errorMessage: string;
    debugText: string;
}

export interface CatsAndDogsPredictState {
    loading: boolean;
    loaded: boolean;
    results: number[];
    predictedValue: number;
    predictResult: boolean;
    errorMessage: string;
    debugText: string;
}

interface PredictImageCatsAndDogsActionLoading { type: 'CND_PREDICT_IMAGE_LOADING' }

interface PredictImageCatsAndDogsActionLoaded {
    type: 'CND_PREDICT_IMAGE_LOADED';
    results: number[];
    predictedValue: number;
    predictResult: boolean;
    errorMessage: string;
    debugText: string;
}

type KnownAction = PredictImageCatsAndDogsActionLoading | PredictImageCatsAndDogsActionLoaded;


export const actionCreators = {
    tryPredictNumber: (imageData: any, isGrpc: any): AppThunkAction<KnownAction> => (dispatch, getState) => {

        const predictClientType = isGrpc ? 'PredictByGrpc' : 'PredictByRest';

        fetch('api/CatsAndDogsDeep/' + predictClientType, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: '{ "imageData": "' + imageData + '" }'
        })
            .then(response => response.json() as Promise<PredictionResult>)
            .then(data => {
                dispatch({ type: 'CND_PREDICT_IMAGE_LOADED', results: data.results, predictedValue: data.predictedValue, predictResult: data.success, errorMessage: data.errorMessage, debugText: data.debugText });
            });

        dispatch({ type: 'CND_PREDICT_IMAGE_LOADING' });
    }
};

export const reducer: Reducer<CatsAndDogsPredictState> = (state: CatsAndDogsPredictState | undefined, incomingAction: Action):CatsAndDogsPredictState => {
    const action = incomingAction as KnownAction;
    
    switch (action.type) {
        case 'CND_PREDICT_IMAGE_LOADING':
            return { loading: true, loaded: false } as CatsAndDogsPredictState;
        case 'CND_PREDICT_IMAGE_LOADED':
            return { loaded: true, loading: false, predictedValue: action.predictedValue, results: action.results, predictResult: action.predictResult, errorMessage: action.errorMessage, debugText: action.debugText } as CatsAndDogsPredictState;
    }

    return state || { loading: false, loaded: false } as CatsAndDogsPredictState;
};
