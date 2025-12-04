import {check, sleep} from 'k6';
import {Client, StatusOK} from 'k6/net/grpc';
import {htmlReport} from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js';

export const options = {
    scenarios: {
        average_load: {
            executor: 'ramping-vus',
            startVUs: 0,
            stages: [
                {duration: '2m', target: 10},  // разогрев до 10 VUs
                {duration: '3m', target: 20},  // рост до средней нагрузки
                {duration: '10m', target: 20}, // стабильная средняя нагрузка
                {duration: '2m', target: 0},   // плавное снижение
            ],
            gracefulRampDown: '30s',
        },
    },
    thresholds: {
        // grpc_req_failed: ['rate<0.01'],
        grpc_req_duration: [
            'avg<500',
            'p(95)<800',
            'p(99)<1200',
        ],
    },
};

const client = new Client();
client.load(['F:/CSharp/DiplomaDictionary/GrpcService/Protos'], 'concept.proto');

export default function () {
    client.connect('localhost:5281', {plaintext: true});

    // имя метода должно совпадать с package.service/rpc из proto
    const response = client.invoke('concept.SubjectService/GetAllTerms', {});

    check(response, {
        'status is OK': (r) => r && r.status === StatusOK,
    });

    client.close();
    sleep(1);
}

export function handleSummary(data) {
    return {
        'results/average-load_getallterms_report.html': htmlReport(data),
    };
}
