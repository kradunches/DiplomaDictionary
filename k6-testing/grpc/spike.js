import {check, sleep} from 'k6';
import {Client, StatusOK} from 'k6/net/grpc';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

export const options = {
    scenarios: {
        spike_test_grpc: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                {duration: '30s', target: 5},   // лёгкий разогрев
                {duration: '30s', target: 50},  // резкий скачок (spike)
                {duration: '2m', target: 50},  // удержание пика
                {duration: '1m', target: 0},   // быстрый спад до 0
            ],
            gracefulRampDown: '30s',
        },
    },
    thresholds: {
        // grpc_req_failed: ['rate<0.05'],
        grpc_req_duration: [
            'p(95)<2000',
            'p(99)<4000',
        ],
    },
};

const client = new Client();
client.load(['F:/CSharp/DiplomaDictionary/GrpcService/Protos'], 'concept.proto');

export default function () {
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
        'results/spike_getallterms_report.html': htmlReport(data),
    };
}
