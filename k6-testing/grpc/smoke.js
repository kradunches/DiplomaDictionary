import {check, sleep} from 'k6';
import {Client, StatusOK} from 'k6/net/grpc';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

export const options = {
    vus: 3,
    duration: '5m',
    thresholds: {
        grpc_req_duration: [
            'p(95)<500',// 95% запросов быстрее 500мс
            'p(99)<1200',
            'avg<500'
        ],
        // grpc_req_failed: ['rate<0.01'], // не более 1% ошибок
    }
};

const client = new Client();
client.load(['F:/CSharp/DiplomaDictionary/GrpcService/Protos'], 'concept.proto');

export default () => {
    client.connect('localhost:5281', {plaintext: true});

    const response = client.invoke('concept.SubjectService/GetAllTerms', {});

    check(response, {
        'status is OK': (r) => r && r.status === StatusOK,
    });

    client.close();
    sleep(1);
}

export function handleSummary(data) {
    return {
        'results/smoke_getallterms_report.html': htmlReport(data),
    };
}